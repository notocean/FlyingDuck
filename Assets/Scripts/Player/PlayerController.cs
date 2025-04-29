using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVisual), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, ITeleportable, ISaveableObject
{
    public PlayerInfor playerInfor { get; private set; }
    public PlayerVisual playerVisual { get; private set; }
    public PlayerEffectHandler playerEffectHandler { get; private set; }

    [SerializeField] MapSideTeleportEvent mapSideTeleportEvent;
    [SerializeField] GroundCheck groundCheck;

    [SerializeField] float maxEnergy;
    [SerializeField] float baseEnergySpeed;
    [SerializeField] float baseWalkSpeed;
    [SerializeField] float baseFlyForce;
    [SerializeField] float flashTime;
    [SerializeField] float flashSpeed;

    List<KeyValuePair<string, Vector2>> velocityModifiers = new List<KeyValuePair<string, Vector2>>();
    Vector2 externalVelocity = Vector2.zero;
    Vector2 directionMove;
    float flashTimer = 0f;
    bool isFlash = false;

    Rigidbody2D rb2d;
    PlayerInputHandler playerInputHandler;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        playerEffectHandler = GetComponent <PlayerEffectHandler>();

        playerInfor = new PlayerInfor(maxEnergy, baseEnergySpeed, baseWalkSpeed, baseFlyForce);
        playerVisual = GetComponent<PlayerVisual>();
        playerVisual.SetPlayerInfor(playerInfor);

        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    private void Start() {
        playerInputHandler.onWalk += Walk;
        playerInputHandler.onFly += Fly;
        playerInputHandler.onFlash += StartFlash;

        mapSideTeleportEvent.RaiseRegisterEvent(transform);
    }

    private void Update() {
        playerInfor.SetEnergy(playerInfor.Energy + playerInfor.EnergySpeed * Time.deltaTime);
        playerVisual.UpdateVisualEnergy();
    }

    private void FixedUpdate() {
        if (!playerInfor.CanControl) {
            directionMove = Vector2.zero;
        }

        Vector2 velocity = externalVelocity;

        if (playerInfor.OnGround) {
            if (playerInfor.IsWalk) {
                velocity += new Vector2(directionMove.x * playerInfor.WalkSpeed, 0);
            }
            rb2d.velocity = velocity;
        }
        else {
            // Nếu có ngoại lực thì đặt vận tốc
            if (velocity.magnitude > 0) {
                rb2d.velocity = velocity;
            }
        }
    }

    void Walk(bool isWalk, Vector2 dirMove) {
        if (!isWalk || !playerInfor.CanControl) {
            if (playerInfor.IsWalk) {
                directionMove = Vector2.zero;

                playerInfor.SetIsWalk(false);
                playerVisual.UpdateVisualWalk();
            }
            return;
        }

        if (playerInfor.OnGround) {
            if (isFlash) {
                isFlash = false;
                RemoveVelocityModifier("PlayerFlash");
            }

            playerInfor.SetDirMove(dirMove.x == 0 ? playerInfor.DirMove : dirMove.x > 0 ? -1 : 1);
            directionMove = dirMove;
            transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);

            playerInfor.SetIsWalk(true);
            playerVisual.UpdateVisualWalk(directionMove.x);
        }
    }

    void OnGroundHandle(bool onGround) {
        if (playerInfor.OnGround != onGround) {
            playerInfor.SetOnGround(onGround);
            if (playerInfor.OnGround) {
                playerEffectHandler.AddEffect(groundCheck.GetEffect());
            }
            else {
                if (playerInfor.IsWalk)
                    Walk(false, Vector2.zero);
                playerEffectHandler.RemoveEffect(groundCheck.GetEffect());
            }

            playerVisual.UpdateVisualOnGround();
        }
    }

    void Fly(Vector2 dirMove) {
        if (isFlash || !playerInfor.CanControl) return;

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
        if (isFlash || !playerInfor.CanControl) return;

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

        if (!isVertical && playerInfor.CanControl) {
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
                RemoveVelocityModifier("PlayerFlash");
                break;
            }
        }

        playerVisual.UpdateVisualFlash(false, isVertical);
    }

    public void Teleport(Vector2 newPos) {
        transform.position = newPos;
    }

    public void TakeDamage() {
        playerVisual.UpdateVisualDamaged();
    }

    public void SetObjectData(ObjectData data) {
        PlayerData playerData = data as PlayerData;

        transform.position = playerData.pos;
        playerInfor.SetDirMove((int)playerData.viewDir);
        transform.localScale = new Vector3(playerInfor.DirMove, 1, 1);
        rb2d.velocity = playerData.velocity;
        playerInfor.SetEnergy(playerData.energy);
        OnGroundHandle(playerData.onground);
    }

    public ObjectData GetObjectData() {
        return new PlayerData(transform.position, (PlayerMoveDir)playerInfor.DirMove, rb2d.velocity, playerInfor.Energy, playerInfor.OnGround);
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

    private void OnTriggerEnter2D(Collider2D collision) {
        ICollected collected = collision.GetComponent<ICollected>();
        if (collected != null) {
            collected.Collect();
        }
    }

    private void OnEnable() {
        groundCheck.onGroundEvent += OnGroundHandle;
    }

    private void OnDisable() {
        groundCheck.onGroundEvent -= OnGroundHandle;
    }
}
