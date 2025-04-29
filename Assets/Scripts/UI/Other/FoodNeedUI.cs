using TMPro;
using UnityEngine;

public class FoodNeedUI : MonoBehaviour
{
    [SerializeField] TMP_Text foodText;

    private void SetFood(int index = 0) {
        Pharmaceutical pharmaceutical = PharmaceuticalManager.Instance.GetCurrentPharmaceutical();
        if (pharmaceutical != null) {
            foodText.text = pharmaceutical.price.ToString();
        }
        else foodText.text = "";
    }

    private void OnEnable() {
        SetFood();
        PharmaceuticalManager.Instance.pharmaceuticalChanged += SetFood;
    }

    private void OnDisable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged -= SetFood;
    }
}
