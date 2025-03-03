using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAreaType { None, Top, TopMiddle, Middle, Bottom }

public enum InputStateType { Tap, DoubleTap, Drag, Hold, Release }

public enum DragType { None, Top, Left, Right, Down }

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
    Vector2 initialInput = Vector2.zero;
    bool isWalk = false;

    [SerializeField] float dragBeginThreshold = 0.1f;
    [SerializeField] float dragThreshold = 0.5f; 
    [SerializeField] float maxDragTime = 0.5f;
    DragType dragType = DragType.None;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        if (isWalk) {
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
                isWalk = true;
                initialInput = input;
                if (input.magnitude < dragBeginThreshold)
                    StartCoroutine(DetectDrag());
                StartCoroutine(DetectHold());
                break;
            case JoystickState.Drag:
                break;
            case JoystickState.Release:
                if (dragType != DragType.None) {
                    stateType = InputStateType.Drag;
                    HandleDrag();
                }
                else {
                    currentInput.Normalize();

                    if (stateType != InputStateType.Hold) {
                        stateType = InputStateType.Tap;
                    }

                    if (areaType == InputAreaType.Top) {
                        playerController.Fly(Vector2.up);
                    }
                    else if (areaType == InputAreaType.TopMiddle) {
                        playerController.Fly(ConvertToClampedAngle());
                    }
                }

                playerController.StopWalk();

                isWalk = false;
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

    IEnumerator DetectDrag() {
        timer = 0f;
        Vector2 startPos = initialInput;
        float previousDragLength = 0f;
        float previousAngle = 0f;
        bool isBeginDragging = false;
        bool isDragging = false;

        while (timer < maxDragTime) {
            timer += Time.deltaTime;

            Vector2 dragDelta = currentInput - startPos;
            float currentAngle = Mathf.Atan2(dragDelta.y, dragDelta.x) * Mathf.Rad2Deg;

            if (dragDelta.magnitude > dragBeginThreshold) {
                if (!isBeginDragging) {
                    previousAngle = Mathf.Atan2(dragDelta.y, dragDelta.x) * Mathf.Rad2Deg;
                    previousDragLength = dragDelta.magnitude;
                    isBeginDragging = true;
                }

                // drag too far off angle or pull back
                float angleDifference = Mathf.Abs(Mathf.DeltaAngle(previousAngle, currentAngle));
                if (angleDifference > 10f || dragDelta.magnitude < previousDragLength) {
                    break;
                }
            }

            if (dragDelta.magnitude > dragThreshold) {
                if (!isDragging) {
                    isDragging = true;
                    if (currentAngle > 60 && currentAngle <= 120) {
                        dragType = DragType.Top;
                    }
                    else if (currentAngle > -30 && currentAngle <= 30) {
                        dragType = DragType.Right;
                    }
                    else if (currentAngle > -120 && currentAngle <= -60) {
                        dragType = DragType.Down;
                    }
                    else {
                        dragType = DragType.Left;
                    }
                }
            }

            previousAngle = currentAngle;
            yield return null;
        }

        dragType = DragType.None;
    }

    void HandleDrag() {
        switch (dragType) {
            case DragType.Top:
                playerController.StartFlash(1.25f * Vector2.up);
                break;
            case DragType.Right:
                playerController.StartFlash(Vector2.right);
                break;
            case DragType.Left:
                playerController.StartFlash(Vector2.left);
                break;
            case DragType.Down:

                break;
        }

        dragType = DragType.None;
    }

    IEnumerator DetectHold() {
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
