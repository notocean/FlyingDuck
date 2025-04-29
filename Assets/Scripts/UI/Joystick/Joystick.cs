using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum JoystickState {
    Press, Drag, Release
}

[RequireComponent(typeof(CanvasGroup))]
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    [SerializeField] PlayerInputEvent playerInputEvent;
    [SerializeField] float handleRange = 1;
    [SerializeField] RectTransform background = null;
    [SerializeField] RectTransform handle = null;

    Canvas canvas;
    CanvasGroup canvasGroup;
    Camera cam;

    [SerializeField] float normalAlpha;
    [SerializeField] float interactiveAlpha;
    [SerializeField] float delayToHideTime;
    float timer = 0f;

    JoystickState state = JoystickState.Release;
    public JoystickState State { get { return state; } }

    Vector2 input = Vector2.zero;
    public Vector2 Input { get { return input; } }

    void Awake() {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        cam = null;
    }

    void Start() {
        handle.anchoredPosition = Vector2.zero;

        SetVisual(normalAlpha);
    }

    public void OnPointerDown(PointerEventData eventData) {
        InputHandle(eventData);
        state = JoystickState.Press;
        playerInputEvent.RaiseJoystickStateChanged(state, input);

        StopAllCoroutines();
        SetVisual(interactiveAlpha);
    }

    public void OnDrag(PointerEventData eventData) {
        InputHandle(eventData);
        state = JoystickState.Drag;
        playerInputEvent.RaiseJoystickStateChanged(state, input);
    }

    public void OnPointerUp(PointerEventData eventData) {
        state = JoystickState.Release;
        playerInputEvent.RaiseJoystickStateChanged(state, input);

        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        StartCoroutine(CountTimeVisual());
    }

    void InputHandle(PointerEventData eventData) {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        if (input.magnitude > 1)
            input = input.normalized;
        handle.anchoredPosition = input * radius * handleRange;
    }

    IEnumerator CountTimeVisual() {
        timer = 0f;

        while (timer < delayToHideTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeVisual(normalAlpha, delayToHideTime));
    }

    IEnumerator FadeVisual(float targetAlpha, float fadeTime) {
        timer = 0f;
        float startAlpha = canvasGroup.alpha;

        while (timer < fadeTime) {
            timer += Time.deltaTime;
            SetVisual(Mathf.Lerp(startAlpha, targetAlpha, timer / fadeTime));
            yield return null;
        }

        SetVisual(targetAlpha);
    }

    void SetVisual(float alpha) {
        canvasGroup.alpha = alpha;
    }
}