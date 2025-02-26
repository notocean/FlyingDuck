using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerInputEvent", menuName = "Events/PlayerInputEvent")]
public class PlayerInputEvent : ScriptableObject
{
    public event Action<JoystickState, Vector2> OnJoystickStateChanged;

    public void RaiseJoystickStateChanged(JoystickState joystickState, Vector2 input) {
        OnJoystickStateChanged.Invoke(joystickState, input);
    }
}
