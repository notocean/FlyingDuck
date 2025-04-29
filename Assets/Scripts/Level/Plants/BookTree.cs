using System.Collections;
using UnityEngine;

public class BookTree : MonoBehaviour, ICollected, IHidableObject
{
    [SerializeField] Pharmaceutical pharmaceutical;
    [SerializeField] float delayToCheck;
    [SerializeField] Sprite checkedBookSprite;
    [SerializeField] GameObject itemCollectedEffectPrefab;
    [SerializeField] AudioClip checkAudioClip;

    const string label = "Công thức mới";
    bool isCollected = false;

    private void Start() {
        DialogManager.Instance.RegisterDialog(itemCollectedEffectPrefab.name, itemCollectedEffectPrefab);
        Init();
    }

    void Init() {
        if (pharmaceutical.isActive) {
            Checked();
        }
    }

    void Checked() {
        GetComponent<SpriteRenderer>().sprite = checkedBookSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        isCollected = true;
    }

    public void Collect() {
        if (!isCollected) {
            isCollected = true;
            StartCoroutine(Check());
        }
    }

    IEnumerator Check() {
        yield return new WaitForSeconds(delayToCheck);

        SoundFXManager.Instance.PlaySoundFX(checkAudioClip);
        DialogManager.Instance.ShowDialog(itemCollectedEffectPrefab.name, new ItemCollectedEffectDialogParamater(label, pharmaceutical.sprite));
        PharmaceuticalManager.Instance.SetActive(pharmaceutical.index);
        PharmaceuticalManager.Instance.UpdateAttention();

        Checked();
    }

    public void SetVisible(bool isVisible) {
        if (isVisible && isCollected) {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
