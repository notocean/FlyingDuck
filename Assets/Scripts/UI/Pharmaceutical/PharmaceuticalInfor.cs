using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PharmaceuticalInfor : MonoBehaviour
{
    [SerializeField] TMP_Text inforText;

    private void Start() {
        SetText();
    }

    private void SetText(int index = 0) {
        inforText.text = PharmaceuticalList.Instance.GetCurrentPharmaceutical().infor;
    }

    private void OnEnable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SetText);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.RemoveListener(SetText);
    }
}
