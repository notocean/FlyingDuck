using System.Collections;
using UnityEngine;

public class CandyTile : MonoBehaviour
{
    [SerializeField] NoControlEffect effect;
    [SerializeField] float effectTime;
    [SerializeField] float force;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot")) {
            Bounce(collision);
        }
    }

    void Bounce(Collision2D collision) {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (!playerController.playerInfor.IsImmune) {
            Rigidbody2D rb = collision.rigidbody;

            Vector2 normal = Vector2.up;
            Vector2 reflectedVelocity = Vector2.Reflect(collision.relativeVelocity, normal).normalized;
            if (reflectedVelocity.y < 0) return;
            if (reflectedVelocity.magnitude == 0) {
                reflectedVelocity = Vector2.up;
            }

            rb.AddForce(force * reflectedVelocity);

            PlayerEffectHandler playerEffectHandler = collision.gameObject.GetComponent<PlayerEffectHandler>();
            StartCoroutine(CountTimeEffect(playerEffectHandler));
        }
    }

    IEnumerator CountTimeEffect(PlayerEffectHandler playerEffectHandler) {
        playerEffectHandler.AddEffect(effect);
        yield return new WaitForSeconds(effectTime);
        playerEffectHandler.RemoveEffect(effect);
    }
}
