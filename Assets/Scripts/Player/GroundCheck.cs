using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Effect effect;

    public Action<bool> onGroundEvent;
    bool hasRegisterPos = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        onGroundEvent?.Invoke(true);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (hasRegisterPos) return;

        int posIndex = TileManager.Instance.GetPosIndexByName(collision.gameObject.name);
        if (posIndex != -1) {
            hasRegisterPos = TileManager.Instance.RegisterPos(posIndex);
        }
        else hasRegisterPos = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        onGroundEvent?.Invoke(false);
        TileManager.Instance.UnregisterPos(TileManager.Instance.GetPosIndexByName(collision.gameObject.name));
    }

    public Effect GetEffect() => effect;
}
