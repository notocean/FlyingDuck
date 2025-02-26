using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MapSideTeleportEvent", menuName = "Events/MapSideTeleportEvent")]
public class MapSideTeleportEvent : ScriptableObject {
    [HideInInspector] public UnityEvent<Transform> RegisterEvent = new UnityEvent<Transform>();

    public void RaiseRegisterEvent(Transform transform) {
        RegisterEvent?.Invoke(transform);
    }
}

