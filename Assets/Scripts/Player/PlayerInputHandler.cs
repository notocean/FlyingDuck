using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAreaType { None, Top, TopMiddle, Middle, Bottom }

public enum InputStateType { Tap, DoubleTap, Hold, Release }

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerInputEvent playerInputEvent;
    PlayerController playerController;

    InputAreaType areaType = InputAreaType.None;
    InputStateType stateType = InputStateType.Release;
    float inputAngle = 0f;

    [SerializeField] float angleOfTop;
    [SerializeField] float angleOfTopMiddle;
    [SerializeField] float angleOfBottom;
    [SerializeField] float clampedAngle;                // the character just flies with this angle, larger angle must be converted to

    float timer = 0f;
    Vector2 currentInput = Vector2.zero;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        if (stateType == InputStateType.Hold) {
            if (areaType >= InputAreaType.Middle) {
                playerController.Walk(currentInput);
            }
            else {
                playerController.StopWalk();
            }
        }
    }

    void OnJoystickInputChanged(JoystickState joystickState, Vector2 input) {
        currentInput = input;

        areaType = Input2AreaType(input);

        switch (joystickState) {
            case JoystickState.Press:
                StartCoroutine(CountTime());
                break;
            case JoystickState.Drag:
                break;
            case JoystickState.Release:
                currentInput.Normalize();

                if (stateType != InputStateType.Hold) {
                    stateType = InputStateType.Tap;
                }

                if (areaType == InputAreaType.Top) {
                    currentInput = Vector2.up;
                    playerController.Fly(currentInput);
                }
                else if (areaType == InputAreaType.TopMiddle) {
                    currentInput = ConvertToClampedAngle();
                    playerController.Fly(currentInput);
                }
                else {
                    playerController.StopWalk();
                }

                stateType = InputStateType.Release;
                StopAllCoroutines();

                break;
        }
    }

    InputAreaType Input2AreaType(Vector2 input) {
        if (input.magnitude == 0)
            return InputAreaType.None;

        input = input.normalized;
        inputAngle = Mathf.Rad2Deg * Mathf.Asin(input.x);

        if (input.y > 0) {
            if (Mathf.Abs(inputAngle) <= angleOfTop)
                return InputAreaType.Top;
            else if (Mathf.Abs(inputAngle) <= angleOfTopMiddle)
                return InputAreaType.TopMiddle;
            else return InputAreaType.Middle;
        }
        else {
            if (Mathf.Abs(inputAngle) <= angleOfBottom)
                return InputAreaType.Bottom;
            else return InputAreaType.Middle;
        }
    }

    // convert vector to this if that vector has an angle that is larger than the clamped angle
    Vector2 ConvertToClampedAngle() {
        float newAngle = (-clampedAngle + (inputAngle + angleOfTopMiddle) * (clampedAngle / angleOfTopMiddle)) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(newAngle), Mathf.Cos(newAngle));
    }

    IEnumerator CountTime() {
        timer = 0f;
        while (stateType != InputStateType.Hold && timer < GameSettings.Instance.holdPoint) {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= GameSettings.Instance.holdPoint) {
                stateType = InputStateType.Hold;
                break;
            }
        }
    }

    private void OnEnable() {
        playerInputEvent.OnJoystickStateChanged += OnJoystickInputChanged;
    }

    private void OnDisable() {
        playerInputEvent.OnJoystickStateChanged -= OnJoystickInputChanged;
    }
}
