using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickedEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform rectTrans;
    [SerializeField] float effectSpeed;
    [SerializeField] float effectScale;
    bool isPointerDown = false;

    private void Awake() {
        rectTrans = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPointerDown = true;
        StopAllCoroutines();
        StartCoroutine(ClickedEffect());
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPointerDown = false;
    }

    IEnumerator ClickedEffect() {
        float timer = (1 - rectTrans.localScale.x) / (1 - effectScale);
        float startScale = rectTrans.localScale.x;
        float targetScale = effectScale;

        // stage 1
        while (timer < 1) {
            timer += Time.unscaledDeltaTime * effectSpeed;
            float scale = Mathf.Lerp(startScale, targetScale, timer);
            rectTrans.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTrans.localScale = new Vector3(effectScale, effectScale, effectScale);

        // wait for pointer up
        yield return new WaitUntil(() => !isPointerDown);

        // stage 2
        timer = 0;
        startScale = effectScale;
        targetScale = 1;

        while (timer < 1) {
            timer += Time.unscaledDeltaTime * effectSpeed;
            float scale = Mathf.Lerp(startScale, targetScale, timer);
            rectTrans.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTrans.localScale = Vector3.one;
    }
}
