using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSideTeleportation : MonoBehaviour
{
    [SerializeField] MapSideTeleportEvent mapSideTeleportEvent;

    HashSet<Transform> teleportableTransforms = new HashSet<Transform>();
    Vector2 pos;

    float minHorizontalWorld;
    float maxHorizontalWorld;

    bool shouldWarp;

    private void Awake() {
        minHorizontalWorld = GameSettings.Instance.minHorizontalWorld;
        maxHorizontalWorld = GameSettings.Instance.maxHorizontalWorld;
    }

    private void RegisterTeleportableObject(Transform transform) {
        if (transform.GetComponent<ITeleportable>() != null) {
            if (!teleportableTransforms.Contains(transform)) {
                teleportableTransforms.Add(transform);
            }
        }
    }

    void UnregisterTeleportableObject(Transform transform) {
        if (teleportableTransforms.Contains(transform)) {
            teleportableTransforms.Remove(transform);
        }
    }

    private void FixedUpdate() {
        foreach (Transform t in teleportableTransforms) {
            pos = t.position;
            if (pos.x >= maxHorizontalWorld)
                WarpHorizontal(minHorizontalWorld + pos.x - maxHorizontalWorld);
            if (pos.x < minHorizontalWorld)
                WarpHorizontal(maxHorizontalWorld - minHorizontalWorld + pos.x);
            if (shouldWarp) {
                t.GetComponent<ITeleportable>().Teleport(pos);
                shouldWarp = false;
            }
        }
    }

    private void WarpHorizontal(float newX) {
        pos.x = newX;
        shouldWarp = true;
    }

    private void OnEnable() {
        mapSideTeleportEvent.RegisterEvent += RegisterTeleportableObject;
        mapSideTeleportEvent.UnregisterEvent += UnregisterTeleportableObject;
    }

    private void OnDisable() {
        mapSideTeleportEvent.RegisterEvent -= RegisterTeleportableObject;
        mapSideTeleportEvent.RegisterEvent -= UnregisterTeleportableObject;
    }
}
