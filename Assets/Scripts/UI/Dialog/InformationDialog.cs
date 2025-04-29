using UnityEngine;
using UnityEngine.UI;

public class InformationDialog : NotificationDialog
{
    [SerializeField] Image image;

    public override void Init(DialogParamater paramater) {
        InformationDialogParamater param = paramater as InformationDialogParamater;
        label.text = param.label;
        image.sprite = param.illustration;
        content.text = param.description;
    }
}

public class InformationDialogParamater : DialogParamater {
    public string label;
    public Sprite illustration;
    public string description;

    public InformationDialogParamater(string label, Sprite illustration, string description) {
        this.label = label;
        this.illustration = illustration;
        this.description = description;
    }
}