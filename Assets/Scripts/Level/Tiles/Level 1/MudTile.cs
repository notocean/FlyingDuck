using UnityEngine;

public class MudTile : MonoBehaviour
{
    [SerializeField] MudEffect mudEffect;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot")) {
            IEffectHandler effectHandler = collision.gameObject.GetComponent<IEffectHandler>();
            if (effectHandler != null) {
                effectHandler.AddEffect(mudEffect);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot")) {
            IEffectHandler effectHandler = collision.gameObject.GetComponent<IEffectHandler>();
            if (effectHandler != null) {
                effectHandler.RemoveEffect(mudEffect);
            }
        }
    }
}
