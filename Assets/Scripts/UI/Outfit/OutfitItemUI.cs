using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class OutfitItemUI : MonoBehaviour
{
    private HairOutfit hairOutfit;

    private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private Image outfitImage;
    private Button outfitButton;

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

    public void ClickedHandle() {
        HairOutfitList.Instance.SetOutfit(hairOutfit.index);
    }
}
