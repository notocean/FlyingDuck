using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EffectShower : MonoBehaviour
{
    private Button button;
    [SerializeField] GameObject informationDialogPrefab;
    [SerializeField] Image image;

    const string label = "THÔNG TIN";
    Sprite illustration;
    string description;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(ShowInfor);

        DialogManager.Instance.RegisterDialog(informationDialogPrefab.name, informationDialogPrefab);
    }

    public void Initial(Sprite illustration, string description) {
        this.illustration = illustration;
        this.description = description;

        image.sprite = illustration;
    }

    private void ShowInfor() {
        DialogManager.Instance.ShowDialog(informationDialogPrefab.name, new InformationDialogParamater(label, illustration, description));
    }
}