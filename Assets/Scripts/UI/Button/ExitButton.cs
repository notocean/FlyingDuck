using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button button;
    [SerializeField] Dialog currentDialog;
    [SerializeField] GameObject dialogPrefab;

    const string warningLabel = "THOÁT TRÒ CHƠI?";
    const string warningContent = "Bạn có chắc chắn muốn thoát trò chơi hay không?";

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
            GameManager.Instance.QuitGame();
        }
    }
}
