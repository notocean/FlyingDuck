using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FoodUI : MonoBehaviour
{
    [SerializeField] TMP_Text foodText;
    [SerializeField] float effectSpeed;
    [SerializeField] float effectScale;
    RectTransform rectTrans;

    bool isEffect = false;

    private void Awake() {
        rectTrans = GetComponent<RectTransform>();
    }

    private void SetFood(int value) {
        foodText.text = value.ToString();
        if (!isEffect)
            StartCoroutine(FoodCollectedEffect());
    }

    IEnumerator FoodCollectedEffect() {
        isEffect = true;
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
        isEffect = false;
    }

    private void OnEnable() {
        SetFood(PlayerData.Instance.Food);
        PlayerData.Instance.foodChanged.AddListener(SetFood);
    }

    private void OnDisable() {
        PlayerData.Instance.foodChanged.RemoveListener(SetFood);
    }
}
