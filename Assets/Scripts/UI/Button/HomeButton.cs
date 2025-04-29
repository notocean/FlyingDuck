using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HomeButton : MonoBehaviour
{
    private Button button;
    [SerializeField] Dialog currentDialog;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OnHome);
    }

    private void OnHome() {
        GameManager.Instance.SaveLevel();
        currentDialog.Close();

        GameManager.Instance.ChangeScene(0);
    }
}
