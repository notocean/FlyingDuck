using System.Collections;
using UnityEngine;

public enum BrokenTileState {
    Idle, Breaking, Rebuilding
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent (typeof(Animator))]    
public class BrokenTile : MonoBehaviour, ISaveableObject 
{
    [SerializeField] float maxStandingTime;
    [SerializeField] float timeToRebuild;
    BrokenTileState state = BrokenTileState.Idle;
    private float timer = 0f;

    private Collider2D col;
    private Animator animator;

    private void Awake() {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot") && state == BrokenTileState.Idle) {
            StartCoroutine(CountDownToBreak());
        }
    }

    IEnumerator CountDownToBreak() {
        state = BrokenTileState.Breaking;
        while (timer <= maxStandingTime) {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("Breaking");
        timer = 0f;
    }

    IEnumerator CountDownToRebuild() {
        animator.SetTrigger("Break");
        state = BrokenTileState.Rebuilding;
        while (timer <= timeToRebuild) {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("Rebuilding");
        timer = 0f;
    }

    public void Break() {
        col.enabled = false;
    }

    public void WaitToRebuild() {
        StartCoroutine(CountDownToRebuild());
    }

    public void Rebuild() {
        col.enabled = true;
    }

    public void Rebuilded() {
        state = BrokenTileState.Idle;
    }

    public ObjectData GetObjectData() {
        return new BrokenTileData(state, timer);
    }

    public void SetObjectData(ObjectData data) {
        BrokenTileData brokenTileData = data as BrokenTileData;
        state = brokenTileData.state;
        timer = brokenTileData.timer;

        if (state == BrokenTileState.Breaking) {
            if (timer != 0f)
                StartCoroutine(CountDownToBreak());
            else StartCoroutine(CountDownToRebuild());
        }
        else if (state == BrokenTileState.Rebuilding) {
            StartCoroutine(CountDownToRebuild());
        }
    }
}

public class BrokenTileData : ObjectData {
    public BrokenTileState state { get; }
    public float timer { get; }

    public BrokenTileData(BrokenTileState state, float timer) {
        this.state = state;
        this.timer = timer;
    }
}
