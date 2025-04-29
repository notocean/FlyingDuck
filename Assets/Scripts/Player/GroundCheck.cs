using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Effect effect;

    public Action<bool> onGroundEvent;
    bool hasRegisterPos = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        onGroundEvent?.Invoke(true);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (hasRegisterPos) return;

        int posIndex = TileManager.Instance.GetPosIndexByName(collision.name);
        if (posIndex != -1) {
            hasRegisterPos = TileManager.Instance.RegisterPos(posIndex);
        }
        else hasRegisterPos = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        onGroundEvent?.Invoke(false);
        TileManager.Instance.UnregisterPos(TileManager.Instance.GetPosIndexByName(collision.name));
    }

    public Effect GetEffect() => effect;
}
