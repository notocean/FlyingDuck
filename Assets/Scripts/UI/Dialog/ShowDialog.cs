using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowDialog : MonoBehaviour
{
    [SerializeField] GameObject dialogPrefab;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(Show);

        DialogManager.Instance.RegisterDialog(dialogPrefab.name, dialogPrefab);
    }

    private void Show() {
        DialogManager.Instance.ShowDialog(dialogPrefab.name);
    }
}
