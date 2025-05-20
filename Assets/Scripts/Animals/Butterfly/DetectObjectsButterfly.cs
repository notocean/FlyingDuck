using System;
using UnityEngine;

public class DetectObjectsButterfly : MonoBehaviour
{
    public event Action<Collider2D, bool> OnTrigger;

    private void OnTriggerEnter2D(Collider2D collision) {
        OnTrigger?.Invoke(collision, true);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        OnTrigger?.Invoke(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        OnTrigger?.Invoke(collision, false);
    }
}
