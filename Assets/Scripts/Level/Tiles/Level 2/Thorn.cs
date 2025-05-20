using System.Collections;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    [SerializeField] NoControlEffect effect;
    [SerializeField] float forceEffectTime = 1f;
    [SerializeField] float force;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot")) {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            PlayerEffectHandler playerEffectHandler = playerController.playerEffectHandler;
            PlayerInfor playerInfor = playerController.playerInfor;

            if (!playerInfor.IsImmune) {
                Rigidbody2D playerRb = collision.rigidbody;

                Vector2 posToPlayerVector = (playerRb.position - (Vector2)transform.position);
                float angle = Vector2.Angle(posToPlayerVector, Vector2.up);
                float clampedAngle = CustomMathf.MapValue(angle, 0, 100f, 20f, 60f) * (transform.position.x > playerRb.position.x ? -1 : 1) * Mathf.Deg2Rad;
                Vector2 clampedVector = new Vector2(Mathf.Sin(clampedAngle), Mathf.Cos(clampedAngle)).normalized;

                playerController.RemoveAllVelocityModifier();
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
