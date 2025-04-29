using UnityEngine;

public class UseButton : HidableButton {
    IEffectHandler effectHandler;

    protected override void Awake() {
        base.Awake();
        effectHandler = GameManager.Instance.Player.GetComponent<IEffectHandler>();
    }

    protected override void Start() {
        base.Start();
        SelectPharmaceuticalHandle();
    }

    protected override void ClickedHandle() {
        PharmaceuticalManager.Instance.UsePharmaceutical(effectHandler);
    }

    private void SelectPharmaceuticalHandle(int index = 0) {
        Pharmaceutical pharmaceutical = PharmaceuticalManager.Instance.GetCurrentPharmaceutical();

        if (pharmaceutical != null) {
            if (pharmaceutical.timeRemainingList[GameManager.Instance.LevelIndex - 1] > 0) {
                SetVisual(false);
            }
            else {
                if (pharmaceutical.count > 0) {
                    SetVisual(true);
                }
                else SetVisual(false);
            }
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
