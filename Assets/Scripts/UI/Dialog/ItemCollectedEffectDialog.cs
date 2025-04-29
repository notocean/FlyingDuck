using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollectedEffectDialog : BasicDialog {
    [SerializeField] TMP_Text labelText;
    [SerializeField] Image image;

    Animator animator;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Open() {
        animator.enabled = true;
        base.Open();
    }

    public override void Close() {
        animator.Rebind();
        animator.enabled = false;
        base.Close();
    }

    public override void Init(DialogParamater paramater) {
        ItemCollectedEffectDialogParamater itemParamater = paramater as ItemCollectedEffectDialogParamater;
        labelText.text = itemParamater.label;
        image.sprite = itemParamater.sprite;
    }
}

public class ItemCollectedEffectDialogParamater : DialogParamater {
    public string label { get; }
    public Sprite sprite { get; }

    public ItemCollectedEffectDialogParamater(string label, Sprite sprite) {
        this.label = label;
        this.sprite = sprite;
    }
}