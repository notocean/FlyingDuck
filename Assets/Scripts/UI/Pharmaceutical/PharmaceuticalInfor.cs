using TMPro;
using UnityEngine;

public class PharmaceuticalInfor : MonoBehaviour
{
    [SerializeField] TMP_Text inforText;

    private void Start() {
        SetText();
    }

    private void SetText(int index = 0) {
        Pharmaceutical pharmaceutical = PharmaceuticalManager.Instance.GetCurrentPharmaceutical();
        if (pharmaceutical != null) {
            inforText.text = pharmaceutical.Infor;
        }
        else inforText.text = "";
    }

    private void OnEnable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged += SetText;
    }

    private void OnDisable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged -= SetText;
    }
}
