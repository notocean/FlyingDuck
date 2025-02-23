using UnityEngine;

public class MapSideTeleportation : MonoBehaviour, ITeleportation
{
    public ITeleportable teleportableObj { get; private set; }
    private Transform playerTrans;
    Vector2 pos;

    float minHorizontalWorld;
    float maxHorizontalWorld;

    bool shouldWarp;

    private void Start() {
        GameObject player = GameManager.Instance.Player;
        if (player != null) {
            playerTrans = player.transform;
            teleportableObj = player.GetComponent<DuckMovement>();
        }
        minHorizontalWorld = GameSettings.Instance.minHorizontalWorld;
        maxHorizontalWorld = GameSettings.Instance.maxHorizontalWorld;
    }

    private void FixedUpdate() {
        pos = playerTrans.position;
        if (pos.x >= maxHorizontalWorld) 
            WarpHorizontal(minHorizontalWorld + pos.x - maxHorizontalWorld);
        if (pos.x < minHorizontalWorld) 
            WarpHorizontal(maxHorizontalWorld - minHorizontalWorld + pos.x);
        if (shouldWarp)
            Teleport();
    }

    private void WarpHorizontal(float newX) {
        pos.x = newX;
        shouldWarp = true;
    }

    public void Teleport() {
        teleportableObj.Teleport(pos);
        shouldWarp = false;
    }
}
