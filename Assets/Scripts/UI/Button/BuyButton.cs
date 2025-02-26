using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : HidableButton {
    protected override void Start() {
        base.Start();
        SelectPharmaceuticalHandle();
    }

    protected override void ClickedHandle() {
        PharmaceuticalList.Instance.Buy();
    }

    private void SelectPharmaceuticalHandle(int index = 0) {
        if (PlayerDataManager.Instance.Food >= PharmaceuticalList.Instance.GetCurrentPharmaceutical().price) {
            SetVisual(true);
        }
        else SetVisual(false);
    }

    private void OnEnable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SelectPharmaceuticalHandle);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SelectPharmaceuticalHandle);
    }
}
