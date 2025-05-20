using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class HoldTile : MonoBehaviour, ISaveableObject 
{
    [SerializeField] HoldEffect holdEffect;
    [SerializeField] AudioClip holdAudioClip;
    float holdTimer = 0f;
    float refreshTimer = 0f;
    bool isHolding = false;
    bool canHold = true;

    Animator animator;
    PlayerEffectHandler playerEffectHandler;

    private void Awake() {
        animator = GetComponent<Animator>();
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    private void Update() {
        if (refreshTimer > 0f) {
            refreshTimer -= Time.deltaTime;
            if (refreshTimer <= 0) {
                canHold = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot") && canHold) {
            playerEffectHandler = collision.gameObject.GetComponent<PlayerEffectHandler>();
            if (playerEffectHandler != null ) {
                Hold(playerEffectHandler);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("PlayerFoot")) {
            if (playerEffectHandler != null) {
                if (playerEffectHandler == collision.gameObject.GetComponent<PlayerEffectHandler>()) {
                    if (isHolding) {
                        animator.SetBool("Hold", false);
                        StopHold();
                    }
                }
            }
        }
    }

    void Hold(PlayerEffectHandler playerEffectHandler) {
        if (!playerEffectHandler.playerInfor.IsImmune) {
            playerEffectHandler.AddEffect(holdEffect);
            holdTimer = holdEffect.HoldTime;
            refreshTimer = 0f;
            isHolding = true;
            canHold = false;
            animator.SetBool("Hold", true);
            SoundFXManager.Instance.PlaySoundFX(holdAudioClip);
            StartCoroutine(CountHoldTime());
        }
    }

    IEnumerator CountHoldTime() {
        while (holdTimer > 0f) {
            holdTimer -= Time.deltaTime;
            yield return null;
        }

        animator.SetBool("Hold", false);
        StopHold();
    }

    void StopHold() {
        if (playerEffectHandler != null) {
            playerEffectHandler.RemoveEffect(holdEffect);
            isHolding = false;
            refreshTimer = holdEffect.RefreshTime;
        }
    }

    public ObjectData GetObjectData() {
        return new HoldTileData(playerEffectHandler == null ? "" : playerEffectHandler.name, holdTimer, refreshTimer);
    }

    public void SetObjectData(ObjectData data) {
        HoldTileData _data = data as HoldTileData;
        holdTimer = _data.holdTimer;
        refreshTimer = _data.refreshTimer;
        canHold = false;

        if (holdTimer > 0f) {
            isHolding = true;
            playerEffectHandler = GameObject.Find(_data.playerName).GetComponent<PlayerEffectHandler>();
            playerEffectHandler.AddEffect(holdEffect);
            StartCoroutine(CountHoldTime());
            animator.SetBool("Hold", true);
            animator.SetTrigger("Is Holding");
        }
        else {
            isHolding = false;
            if (refreshTimer <= 0f) {
                canHold = true;
            }
        }
    }
}

public class HoldTileData : ObjectData {
    public string playerName { get; }
    public float holdTimer { get; }
    public float refreshTimer { get; }

    public HoldTileData(string playerName, float holdTimer, float refreshTimer) {
        this.playerName = playerName;
        this.holdTimer = holdTimer;
        this.refreshTimer = refreshTimer;
    }

    public override ObjectData Clone() {
        return new HoldTileData(playerName, holdTimer, refreshTimer);
    }
}
