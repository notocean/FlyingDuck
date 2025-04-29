using UnityEngine;
using UnityEngine.UI;

public class BasicDialog : Dialog 
{
    [SerializeField] protected Button closeBtn;

    protected virtual void Start() {
        closeBtn.onClick.AddListener(HideDialog);
    }

    protected virtual void HideDialog() {
        DialogManager.Instance.HideDialog();
    }
}
