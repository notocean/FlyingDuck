using UnityEngine;

public class Item : MonoBehaviour, ICollected
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int itemIndex;
    [SerializeField] GameObject itemCollectedEffectPrefab;
    AudioPlayer audioPlayer;
    bool isCollected = false;

    const string label = "Trang phục mới";

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioPlayer = GetComponentInChildren<AudioPlayer>();
    }

    private void Start() {
        DialogManager.Instance.RegisterDialog(itemCollectedEffectPrefab.name, itemCollectedEffectPrefab);
        Initial();
    }

    public void Initial() {
        if (HairOutfitManager.Instance.hairOutfits[itemIndex].isActive) {
            isCollected = true;
            Destroy(gameObject);
        }
        spriteRenderer.sprite = HairOutfitManager.Instance.hairOutfits[itemIndex].spriteUI;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
    }

    public void Collect() {
        if (!isCollected) {
            isCollected = true;

            GetComponent<Collider2D>().enabled = false;
            audioPlayer.Play();
            DialogManager.Instance.ShowDialog(itemCollectedEffectPrefab.name, new ItemCollectedEffectDialogParamater(label, spriteRenderer.sprite));

            HairOutfitManager.Instance.SetActive(itemIndex);
            HairOutfitManager.Instance.UpdateAttention();
            Destroy(gameObject);
        }
    }
}
