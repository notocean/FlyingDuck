using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationDialogParamater : DialogParamater {
    public string label;
    public string content;
    public NotificationDialogParamater(string label, string content) {
        this.label = label;
        this.content = content;
    }
}

public class NotificationDialog : Dialog
{
    protected TaskCompletionSource<bool> tcs;

    [SerializeField] protected Button closeBtn;
    [SerializeField] protected TMP_Text label;
    [SerializeField] protected TMP_Text content;

    protected virtual void Start() {
        closeBtn.onClick.AddListener(HideDialog);
    }

    public override void Init(DialogParamater paramater) {
        NotificationDialogParamater param = paramater as NotificationDialogParamater;
        label.text = param.label;
        content.text = param.content;
    }

    public async Task<bool> WaitAsync() {
        tcs = new TaskCompletionSource<bool>();
        return await tcs.Task;
    }

    public override void Close() {
        tcs?.TrySetResult(false);
        base.Close();
    }

    protected virtual void HideDialog() {
        DialogManager.Instance.HideDialog();
    }
}
