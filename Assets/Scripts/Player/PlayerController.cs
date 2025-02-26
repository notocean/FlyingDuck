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
    Vector2 groundVelocity = Vector2.zero;
    Vector2 directionMove;

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
        if (playerInfor.OnGround) {
            //Vector2 velocity = Vector2.zero;
            //velocity += groundVelocity;
            //if (playerInfor.IsWalk) {
            //    velocity += new Vector2(playerInfor.MoveDir * playerInfor.WalkSpeed, 0);
            //}
            //rb2d.velocity = velocity;

            Vector2 velocity = Vector2.zero;
            if (playerInfor.IsWalk) {
                velocity = new Vector2(directionMove.x * playerInfor.WalkSpeed, 0);
            }
            rb2d.velocity = velocity;
        }
    }

    public void Walk(Vector2 dirMove) {
        playerInfor.SetMoveDir(dirMove.x == 0 ? playerInfor.MoveDir : dirMove.x > 0 ? -1 : 1);
        directionMove = dirMove;
        transform.localScale = new Vector3(playerInfor.MoveDir, 1, 1);

        playerInfor.SetIsWalk(true);
        playerVisual.UpdateVisualWalk(directionMove.x);
    }

    public void StopWalk() {
        if (playerInfor.IsWalk) {
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
        if (playerInfor.Energy >= 1f) {
            playerInfor.SetMoveDir(dirMove.x == 0 ? playerInfor.MoveDir : dirMove.x > 0 ? -1 : 1);
            playerInfor.SetEnergy(playerInfor.Energy - 1);
            rb2d.velocity = Vector2.zero;
            groundVelocity = Vector2.zero;

            transform.localScale = new Vector3(playerInfor.MoveDir, 1, 1);
            rb2d.AddForce(dirMove * playerInfor.FlyForce);
            playerVisual.UpdateVisualFly();
        }
    }

    public void Teleport(Vector2 newPos) {
        transform.position = newPos;
    }

    public void SetPlayerData(PlayerData playerData) {
        transform.position = playerData.pos;
        playerInfor.SetMoveDir((int)playerData.viewDir);
        rb2d.velocity = playerData.velocity;
        playerInfor.SetEnergy(playerData.energy);
    }

    public PlayerData GetPlayerData() {
        return new PlayerData(transform.position, (PlayerMoveDir)playerInfor.MoveDir, rb2d.velocity, playerInfor.Energy);
    }

    private void OnEnable() {
        groundCheck.onGroundEvent.AddListener(OnGroundHandle);
    }

    private void OnDisable() {
        groundCheck.onGroundEvent.RemoveListener(OnGroundHandle);
    }

    public void SetGroundVelocity(Vector2 velocity) {
        groundVelocity = velocity;
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
