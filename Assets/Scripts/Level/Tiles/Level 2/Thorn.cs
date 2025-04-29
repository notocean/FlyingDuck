using System.Collections;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    [SerializeField] NoControlEffect effect;
    [SerializeField] float forceEffectTime = 1f;
    [SerializeField] float force;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot")) {
            PlayerEffectHandler playerEffectHandler = collision.GetComponentInParent<PlayerEffectHandler>();
            PlayerInfor playerInfor = playerEffectHandler.playerInfor;

            if (!playerInfor.IsImmune) {
                Rigidbody2D playerRb = collision.GetComponentInParent<Rigidbody2D>();

                Vector2 posToPlayerVector = (playerRb.position - (Vector2)transform.position).normalized;
                float angle = Vector2.Angle(posToPlayerVector, Vector2.up);
                float clampedAngle = CustomMathf.MapValue(angle, 0, 100f, 20f, 60f) * (transform.position.x > playerRb.position.x ? -1 : 1) * Mathf.Deg2Rad;
                Vector2 clampedVector = new Vector2(Mathf.Sin(clampedAngle), Mathf.Cos(clampedAngle));

                playerRb.velocity = Vector2.zero;
                playerRb.AddForce(force * clampedVector);

                StartCoroutine(CountTimeEffect(playerEffectHandler));
            }
        }
    }

    IEnumerator CountTimeEffect(PlayerEffectHandler playerEffectHandler) {
        playerEffectHandler.AddEffect(effect);
        playerEffectHandler.GetComponent<PlayerController>().TakeDamage();
        yield return new WaitForSeconds(forceEffectTime);
        playerEffectHandler.RemoveEffect(effect);
    }
}
