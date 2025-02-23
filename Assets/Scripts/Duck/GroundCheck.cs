using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] AudioClip audioClip;

    [HideInInspector] public UnityEvent<bool> onGroundEvent = new UnityEvent<bool>();
    private bool onGround = false;

    private void Start() {
        onGroundEvent.Invoke(onGround = false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0) {
            onGroundEvent.Invoke(onGround = true);
            AudioManager.Instance.PlayOneShot(audioClip);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0) {
            onGroundEvent.Invoke(onGround = false);
        }
    }

    public bool OnGround() {
        return onGround;
    }
}
