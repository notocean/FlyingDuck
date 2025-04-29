using System;
using System.Collections;
using UnityEngine;

public enum InputAreaType { None, Top, TopMiddle, Middle, Bottom }

public enum InputStateType { Tap, DoubleTap, Drag, Hold, Release }

public enum DragType { None, Top, Left, Right, Down }

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerInputEvent playerInputEvent;

    [SerializeField] float angleOfTop;
    [SerializeField] float angleOfTopMiddle;
    [SerializeField] float angleOfBottom;
    [SerializeField] float clampAngle;                // Nhân vật chỉ được bay với góc này
    [SerializeField] float deadZone;
    [SerializeField] float dragThreshold;
    [SerializeField] float maxDragTime;

    InputAreaType areaType = InputAreaType.None;
    InputStateType stateType = InputStateType.Release;

    float inputAngle = 0f;
    float timer = 0f;
    Vector2 currentInput = Vector2.zero;
    Vector2 initialInput = Vector2.zero;
    bool isWalk = false;

    DragType dragType = DragType.None;
    // Có một deadzone ở giữa joystick
    // Nếu người dùng bắt đầu kéo từ deadzone thì được tính là kéo
    // Nếu người dùng bắt đầu kéo từ ngoài deadzone thì không được tính
    bool canDragTypeChange = false;

    public Action<bool, Vector2> onWalk;
    public Action<Vector2> onFly;
    public Action<Vector2> onFlash;

    private void Update() {
        if (isWalk) {
            if (areaType >= InputAreaType.Middle) {
                onWalk?.Invoke(true, currentInput);
            }
            else {
                // Dừng di chuyển nếu input ở vị trí bay
                onWalk?.Invoke(false, Vector2.zero);
            }
        }
    }

    void OnJoystickInputChanged(JoystickState joystickState, Vector2 input) {
        currentInput = input;

        areaType = Input2AreaType(input);

        switch (joystickState) {
            case JoystickState.Press:
                initialInput = input;
                if (input.magnitude < deadZone) {
                    canDragTypeChange = true;
                    StartCoroutine(DetectDrag());
                }
                else {
                    canDragTypeChange = false;
                    if (areaType >= InputAreaType.Middle) {
                        isWalk = true;
                    }
                }
                StartCoroutine(DetectHold());
                break;
            case JoystickState.Drag:
                if (!canDragTypeChange || stateType == InputStateType.Hold) {
                    if (areaType >= InputAreaType.Middle) {
                        isWalk = true;
                    }
                }
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
                        onFly?.Invoke(Vector2.up);
                    }
                    else if (areaType == InputAreaType.TopMiddle) {
                        onFly?.Invoke(ConvertToClampedAngle());
                    }
                }

                if (isWalk) {
                    onWalk?.Invoke(false, Vector2.zero);
                    isWalk = false;
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
        inputAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

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

    Vector2 ConvertToClampedAngle() {
        float clampedAngle = CustomMathf.MapValue(inputAngle, 0, angleOfTopMiddle, 0, clampAngle) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(clampedAngle), Mathf.Cos(clampedAngle));
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
            float currentAngle = Mathf.Atan2(dragDelta.x, dragDelta.y) * Mathf.Rad2Deg;

            if (dragDelta.magnitude > deadZone) {
                if (!isBeginDragging) {
                    previousAngle = Mathf.Atan2(dragDelta.x, dragDelta.y) * Mathf.Rad2Deg;
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
                    if (currentAngle >= -30 && currentAngle <= 30) {
                        dragType = DragType.Top;
                    }
                    else if (currentAngle >= 60 && currentAngle <= 120) {
                        dragType = DragType.Right;
                    }
                    else if (currentAngle >= -120 && currentAngle <= -60) {
                        dragType = DragType.Left;
                    }
                    else if (currentAngle >= 150 || currentAngle <= -150) {
                        dragType = DragType.Down;
                    }
                    else dragType = DragType.None;
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
                onFlash?.Invoke(1.25f * Vector2.up);
                break;
            case DragType.Right:
                onFlash?.Invoke(Vector2.right);
                break;
            case DragType.Left:
                onFlash?.Invoke(Vector2.left);
                break;
            case DragType.Down:

                break;
        }

        dragType = DragType.None;
    }

    IEnumerator DetectHold() {
        timer = 0f;

        while (stateType != InputStateType.Hold && timer < GameSettings.Instance.HoldPoint) {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= GameSettings.Instance.HoldPoint) {
                stateType = InputStateType.Hold;
                dragType = DragType.None;
                OnJoystickInputChanged(JoystickState.Press, currentInput);
                StopAllCoroutines();
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
