using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialog : NotificationDialog
{
    [SerializeField] protected Button confirmBtn;

    protected override void Start() {
        base.Start();
        confirmBtn.onClick.AddListener(Confirm);
    }

    public void Confirm() {
        tcs?.TrySetResult(true);
        base.Close();
    }
}
