using TMPro;
using UnityEngine;

public abstract class Animal : MonoBehaviour, IHidableObject, ICollected
{
    [SerializeField] protected int food;
    [SerializeField] protected float timeToRefresh;
    [SerializeField] protected GameObject showCollectedDialogPrefab;
    [SerializeField] AudioClip collectedAudioClip;

    protected Animator animator;
    protected bool alive = true;
    protected float refreshTimer = 0f;
    protected bool isVisible = true;

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
    }

    protected virtual void Spawn() {
        alive = true;

        if (isVisible) {
            SetLocalVisible(true);
        }

        StopAllCoroutines();
    }

    protected virtual void Destroy() {
        alive = false;

        SetLocalVisible(false);

        refreshTimer = timeToRefresh;
        StopAllCoroutines();
    }

    public virtual void Collect() {
        if (alive) {
            Destroy();

            GameObject showCollectedDialog = Instantiate(showCollectedDialogPrefab, transform.position, Quaternion.identity);
            if (showCollectedDialog != null) {
                TMP_Text foodText = showCollectedDialog.GetComponentInChildren<TMP_Text>();
                if (foodText != null) {
                    foodText.text = $"+{food}";
                }
            }

            PlayerDataManager.Instance.Food += food;
            SoundFXManager.Instance.PlaySoundFX(collectedAudioClip);
        }
    }

    protected virtual void SetLocalVisible(bool isVisible) {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>()) {
            sprite.enabled = isVisible;
        }
        GetComponent<Collider2D>().enabled = isVisible;
    }

    public abstract void SetVisible(bool isVisible);
}
