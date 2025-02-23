using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class FoodNeedUI : MonoBehaviour
{
    [SerializeField] TMP_Text foodText;

    private void SetFood(int index = 0) {
        foodText.text = PharmaceuticalList.Instance.GetCurrentPharmaceutical().price.ToString();
    }

    private void OnEnable() {
        SetFood();
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SetFood);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.RemoveListener(SetFood);
    }
}
