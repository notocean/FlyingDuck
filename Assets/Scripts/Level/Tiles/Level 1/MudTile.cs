using UnityEngine;

public class MudTile : MonoBehaviour
{
    [SerializeField] MudEffect mudEffect;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot")) {
            IEffectHandler effectHandler = collision.GetComponentInParent<IEffectHandler>();
            if (effectHandler != null) {
                effectHandler.AddEffect(mudEffect);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot")) {
            IEffectHandler effectHandler = collision.GetComponentInParent<IEffectHandler>();
            if (effectHandler != null) {
                effectHandler.RemoveEffect(mudEffect);
            }
        }
    }
}
