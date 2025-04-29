using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSideTeleportEvent", menuName = "Events/MapSideTeleportEvent")]
public class MapSideTeleportEvent : ScriptableObject {
    public Action<Transform> RegisterEvent;
    public Action<Transform> UnregisterEvent;

    public void RaiseRegisterEvent(Transform transform) {
        RegisterEvent?.Invoke(transform);
    }

    public void RaiseUnregisterEvent(Transform transform) {
        UnregisterEvent?.Invoke(transform);
    }
}

