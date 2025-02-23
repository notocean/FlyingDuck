using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReplayButton : MonoBehaviour 
{
    private Button button;
    [SerializeField] Dialog currentDialog;
    [SerializeField] ConfirmDialog confirmDialog;

    const string warningLabel = "Chơi lại?";
    const string warningContent = "Dữ liệu hiện tại của bạn sẽ bị mất. Bạn có chắc chắn muốn chơi lại không?";

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OnRePlay);
    }

    private async void OnRePlay() {
        currentDialog.Close();

        confirmDialog.Init(new NotificationDialogParamater(warningLabel, warningContent));
        confirmDialog.Open();
        bool isConfirmed = await confirmDialog.WaitAsync();

        if (isConfirmed) {
            GameManager.Instance.ResetLevel();
        }
    }
}
