using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RepeatMovingTile))]
public class GrabTile : MonoBehaviour
{
    Rigidbody2D rb2D;
    RepeatMovingTile repeatMovingTile;
    PlayerController player;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        repeatMovingTile = GetComponent<RepeatMovingTile>();

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        SetPlayer(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (player == null) {
            SetPlayer(collision);
        }
    }

    void SetPlayer(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot") && collision.otherCollider.name.Equals(name)) {
            player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null) {
                player.AddVelocityModifier(name, rb2D.velocity);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot") && collision.otherCollider.name.Equals(name)) {
            if (player != null) {
                player.RemoveVelocityModifier(name);
                player = null;
            }
        }
    }

    void VelocityChangeHandle() {
        if (player != null) {
            player.RemoveVelocityModifier(name);
            player.AddVelocityModifier(name, rb2D.velocity);
        }
    }

    private void OnEnable() {
        repeatMovingTile.OnVelocityChanged += VelocityChangeHandle;
    }

    private void OnDisable() {
        repeatMovingTile.OnVelocityChanged -= VelocityChangeHandle;
    }
}
