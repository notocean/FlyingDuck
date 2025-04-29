using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class OutfitItemUI : MonoBehaviour
{
    public HairOutfit hairOutfit { get; private set; }

    private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private Image outfitImage;
    [SerializeField] GameObject attentionPrefab;

    private Button outfitButton;
    GameObject attentionObj;

    private void Awake() {
        backgroundImage = GetComponent<Image>();
        outfitButton = GetComponent<Button>();
    }

    private void Start() {
        outfitButton.onClick.AddListener(ClickedHandle);
    }

    public void Initial(HairOutfit hairOutfit) {
        this.hairOutfit = hairOutfit;

        SetActive(hairOutfit.isActive);
        outfitImage.sprite = hairOutfit.spriteUI;
    }

    public void SetVisual(bool isSelected) {
        if (isSelected) {
            backgroundImage.sprite = selectedSprite;
        }
        else {
            backgroundImage.sprite = normalSprite;
        }
    }

    public void SetActive(bool isActive) {
        outfitButton.interactable = isActive;
        if (isActive) {
            backgroundImage.color = Color.white;
        }
        else {
            backgroundImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void SetAttention(bool attention) {
        if (attention) {
            if (attentionObj == null) {
                attentionObj = Instantiate(attentionPrefab, transform);
            }
        }
        else {
            if (attentionObj != null) {
                Destroy(attentionObj);
            }
        }
    }

    public void ClickedHandle() {
        HairOutfitManager.Instance.SetOutfit(hairOutfit.index);

        if (hairOutfit.hasAttention) {
            hairOutfit.hasAttention = false;
            SetAttention(false);
            HairOutfitManager.Instance.UpdateAttention();
        }
    }
}
