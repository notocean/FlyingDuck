using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class PharmaceuticalItemUI : MonoBehaviour {
    public Pharmaceutical pharmaceutical { get; private set; }

    private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private Image pharmaceuticalImage;
    [SerializeField] private TMP_Text countText;

    [SerializeField] GameObject attentionPrefab;

    private Button pharmaceuticalButton;
    GameObject attentionObj;

    private void Awake() {
        backgroundImage = GetComponent<Image>();
        pharmaceuticalButton = GetComponent<Button>();
    }

    private void Start() {
        pharmaceuticalButton.onClick.AddListener(ClickedHandle);
    }

    public void Initial(Pharmaceutical pharmaceutical) {
        this.pharmaceutical = pharmaceutical;

        SetActive(pharmaceutical.isActive);
        pharmaceuticalImage.sprite = pharmaceutical.sprite;
        ShowCount(pharmaceutical.index);
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

    private void PharmaceuticalChanged(int index) {
        if (pharmaceutical.index == index) {
            ShowCount(index);
        }
    }

    private void ShowCount(int index) {
        countText.text = PharmaceuticalManager.Instance.pharmaceuticalList[index].count.ToString();
    }

    private void ClickedHandle() {
        PharmaceuticalManager.Instance.SetCurrentPharmaceutical(pharmaceutical.index);

        if (pharmaceutical.hasAttention) {
            pharmaceutical.hasAttention = false;
            SetAttention(false);
            PharmaceuticalManager.Instance.UpdateAttention();
        }
    }

    private void OnEnable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged += PharmaceuticalChanged;
    }

    private void OnDisable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged -= PharmaceuticalChanged;
    }
}
