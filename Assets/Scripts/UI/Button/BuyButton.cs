public class BuyButton : HidableButton {
    protected override void Start() {
        base.Start();
        SelectPharmaceuticalHandle();
    }

    protected override void ClickedHandle() {
        PharmaceuticalManager.Instance.Buy();
    }

    private void SelectPharmaceuticalHandle(int index = 0) {
        Pharmaceutical pharmaceutical = PharmaceuticalManager.Instance.GetCurrentPharmaceutical();
        if (pharmaceutical != null) {
            if (PlayerDataManager.Instance.Food >= pharmaceutical.price) {
                SetVisual(true);
            }
            else SetVisual(false);
        }
        else SetVisual(false);
    }

    private void OnEnable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged += SelectPharmaceuticalHandle;
    }

    private void OnDisable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged -= SelectPharmaceuticalHandle;
    }
}
