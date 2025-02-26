using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class PharmaceuticalItemUI : MonoBehaviour {
    PharmaceuticalItemManager pharmaceuticalItemManager;
    private int index;

    private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private Image pharmaceuticalImage;
    [SerializeField] private TMP_Text countText;
    private Button pharmaceuticalButton;

    private void Awake() {
        backgroundImage = GetComponent<Image>();
        pharmaceuticalButton = GetComponent<Button>();
    }

    private void Start() {
        pharmaceuticalButton.onClick.AddListener(ClickedHandle);
    }

    public void Initial(Pharmaceutical pharmaceutical, PharmaceuticalItemManager pharmaceuticalItemManager, int index) {
        this.pharmaceuticalItemManager = pharmaceuticalItemManager;
        this.index = index;

        SetActive(pharmaceutical.isActive);
        pharmaceuticalImage.sprite = pharmaceutical.sprite;
        ShowCount(index);
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
        pharmaceuticalButton.interactable = isActive;
        if (isActive) {
            backgroundImage.color = Color.white;
        }
        else {
            backgroundImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // will improve later about count
    private void PharmaceuticalChanged(int index) {
        if (this.index == index) {
            ShowCount(index);
        }
    }

    private void ShowCount(int index) {
        countText.text = PharmaceuticalList.Instance.pharmaceuticals[index].count.ToString();
    }

    private void ClickedHandle() {
        PharmaceuticalList.Instance.SetCurrentPharmaceutical(index);
    }

    private void OnEnable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(PharmaceuticalChanged);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.RemoveListener(PharmaceuticalChanged);
    }
}
