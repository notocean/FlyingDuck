using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputEvent", menuName = "Game/InputEvent")]
public class InputEvent : ScriptableObject
{
    [HideInInspector] public UnityEvent<ButtonType, ButtonState> Event = new UnityEvent<ButtonType, ButtonState>();

    public void RaiseEvent(ButtonType buttonType, ButtonState buttonState) {
        Event?.Invoke(buttonType, buttonState);
    }
}
