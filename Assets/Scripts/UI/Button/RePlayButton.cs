using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReplayButton : MonoBehaviour 
{
    private Button button;
    [SerializeField] Dialog currentDialog;
    [SerializeField] GameObject dialogPrefab;

    const string warningLabel = "CHƠI LẠI?";
    const string warningContent = "Dữ liệu hiện tại của bạn sẽ bị mất. Bạn có chắc chắn muốn chơi lại không?";

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OnReplay);

        DialogManager.Instance.RegisterDialog(dialogPrefab.name, dialogPrefab);
    }

    private async void OnReplay() {
        currentDialog.Close();

        ConfirmDialog confirmDialog = DialogManager.Instance.ShowDialog(dialogPrefab.name, new NotificationDialogParamater(warningLabel, warningContent)) as ConfirmDialog;
        bool isConfirmed = await confirmDialog.WaitAsync();

        if (isConfirmed) {
            GameManager.Instance.ResetLevel();
        }
    }
}
