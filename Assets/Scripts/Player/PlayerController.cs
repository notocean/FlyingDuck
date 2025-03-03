using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVisual), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, ITeleportable
{
    public PlayerInfor playerInfor { get; private set; }
    public PlayerVisual playerVisual { get; private set; }

    [SerializeField] MapSideTeleportEvent mapSideTeleportEvent;
    [SerializeField] GroundCheck groundCheck;

    List<KeyValuePair<string, Vector2>> velocityModifiers = new List<KeyValuePair<string, Vector2>>();
    Vector2 externalVelocity = Vector2.zero;

    Vector2 directionMove;

    [SerializeField] float flashTime;
    [SerializeField] float flashSpeed;
    float flashTimer = 0f;
    bool isFlash = false;

    Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();

        playerInfor = new PlayerInfor(4f, 0.5f, 1f, 250f);
        playerVisual = GetComponent<PlayerVisual>();
        playerVisual.SetPlayerInfor(playerInfor);

        mapSideTeleportEvent.RaiseRegisterEvent(transform);
    }

    private void Update() {
        playerInfor.SetEnergy(playerInfor.Energy + playerInfor.EnergySpeed * Time.deltaTime);
        playerVisual.UpdateVisualEnergy();
    }

    private void FixedUpdate() {
        Vector2 velocity = externalVelocity;

        if (playerInfor.OnGround) {
            if (playerInfor.IsWalk) {
                velocity += new Vector2(directionMove.x * playerInfor.WalkSpeed, 0);
            }
            rb2d.velocity = velocity;
        }
        else {
            // if there is external force then set velocity
            if (velocity.magnitude > 0) {
                rb2d.velocity = velocity;
            }
        }
    }

    public void Walk(Vector2 dirMove) {
        if (isFlash) isFlash = false;

        if (playerInfor.OnGround) {
            playerInfor.SetDirMove(dirMove.x == 0 ? playerInfor.DirMove : dirMove.x > 0 ? -1 : 1);
            directionMove = dirMove;
            transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);

            playerInfor.SetIsWalk(true);
            playerVisual.UpdateVisualWalk(directionMove.x);
        }
    }

    public void StopWalk() {
        if (playerInfor.IsWalk) {
            directionMove = Vector2.zero;

            playerInfor.SetIsWalk(false);
            playerVisual.UpdateVisualWalk();
        }
    }

    void OnGroundHandle(bool onGround) {
        playerInfor.SetOnGround(onGround);
        if (playerInfor.OnGround) {
            playerInfor.SetEnergySpeed(playerInfor.DefaultEnergySpeed * 4f);
        }
        else {
            if (playerInfor.IsWalk)
                StopWalk();
            playerInfor.SetEnergySpeed(playerInfor.DefaultEnergySpeed);
        }

        playerVisual.UpdateVisualOnGround();
    }

    public void Fly(Vector2 dirMove) {
        if (isFlash) {
            isFlash = false;
            RemoveVelocityModifier("PlayerFlash");
        }

        if (playerInfor.Energy >= 1f) {
            playerInfor.SetDirMove(dirMove.x == 0 ? playerInfor.DirMove : dirMove.x > 0 ? -1 : 1);
            playerInfor.SetEnergy(playerInfor.Energy - 1);
            rb2d.velocity = Vector2.zero;

            transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);
            rb2d.AddForce(dirMove * playerInfor.FlyForce);
            playerVisual.UpdateVisualFly();
        }
    }

    public void StartFlash(Vector2 flashDirection) {
        if (isFlash) return;

        if (playerInfor.Energy >= 1.5f) {
            playerInfor.SetEnergy(playerInfor.Energy - 1.5f);
            StartCoroutine(Flash(flashDirection));
        }
    }

    IEnumerator Flash(Vector2 flashDirection) {
        isFlash = true;
        flashTimer = 0f;
        rb2d.velocity = Vector2.zero;

        bool isVertical = flashDirection.y > 0;
        playerVisual.UpdateVisualFlash(true, isVertical);

        if (!isVertical) {
            playerInfor.SetDirMove(-(int)flashDirection.x);
            transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);
        }

        AddVelocityModifier("PlayerFlash", flashSpeed * flashDirection);

        while (isFlash) {
            yield return new WaitForFixedUpdate();

            flashTimer += Time.deltaTime;
            if (flashTimer >= flashTime) {
                rb2d.velocity = isVertical ? Vector2.up : Vector2.zero;
                isFlash = false;
                break;
            }
        }

        RemoveVelocityModifier("PlayerFlash");
        playerVisual.UpdateVisualFlash(false, isVertical);
    }

    public void Teleport(Vector2 newPos) {
        transform.position = newPos;
    }

    public void SetPlayerData(PlayerData playerData) {
        transform.position = playerData.pos;
        playerInfor.SetDirMove((int)playerData.viewDir);
        transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);
        rb2d.velocity = playerData.velocity;
        playerInfor.SetEnergy(playerData.energy);
    }

    public PlayerData GetPlayerData() {
        return new PlayerData(transform.position, (PlayerMoveDir)playerInfor.DirMove, rb2d.velocity, playerInfor.Energy);
    }

    private void OnEnable() {
        groundCheck.onGroundEvent.AddListener(OnGroundHandle);
    }

    private void OnDisable() {
        groundCheck.onGroundEvent.RemoveListener(OnGroundHandle);
    }

    // will improve later
    public float GetWalkSpeed() {
        return playerInfor.WalkSpeed;
    }

    public void SetWalkSpeed(float value) {
        playerInfor.SetWalkSpeed(value);
    }

    public float GetFlyForce() {
        return playerInfor.FlyForce;
    }

    public void SetFlyForce(float value) {
        playerInfor.SetFlyForce(value);
    }

    public void AddVelocityModifier(string source, Vector2 velocity) {
        velocityModifiers.Add(new KeyValuePair<string, Vector2>(source, velocity));
        externalVelocity = CalculateExternalVelocity();
    }

    public void RemoveVelocityModifier(string source) {
        velocityModifiers.RemoveAll(mod => mod.Key == source);
        externalVelocity = CalculateExternalVelocity();
    }

    Vector2 CalculateExternalVelocity() {
        Vector2 velocity = Vector2.zero;
        foreach(KeyValuePair<string, Vector2> kvp in velocityModifiers) {
            velocity += kvp.Value;
        }
        return velocity;
    }

    public bool OnGround() {
        return playerInfor.OnGround;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            Animal animal = collision.GetComponent<Animal>();
            if (animal != null) {
                int food = animal.Collected();
                PlayerDataManager.Instance.Food += food;
            }
        }
    }
}
