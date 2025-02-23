using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    [SerializeField] protected ButtonType buttonType;
    private ButtonState _buttonState;

    protected ButtonState buttonState {
        get { return _buttonState; }
        private set {
            _buttonState = value;
            interactEvent?.Invoke(buttonType, _buttonState);
        }
    }

    public UnityEvent<ButtonType, ButtonState> interactEvent;
    protected float timer = 0f;

    public virtual void OnPointerDown(PointerEventData eventData) {
        timer = 0f;
        StartCoroutine(PressHandle());
    }

    public virtual void OnPointerUp(PointerEventData eventData) {
        StopAllCoroutines();
        timer += Time.deltaTime;
        if (timer < GameSettings.Instance.holdPoint) {
            buttonState = ButtonState.Tap;
        }

        buttonState = ButtonState.Release;
    }

    protected virtual IEnumerator PressHandle() {
        while (timer < GameSettings.Instance.holdPoint) {
            yield return null;
            timer += Time.deltaTime;
        }

        buttonState = ButtonState.Hold;
    }
}
