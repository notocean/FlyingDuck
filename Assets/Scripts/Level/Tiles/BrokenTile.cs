using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrokenTileState {
    Idle, Breaking, Rebuilding
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent (typeof(Animator))]    
public class BrokenTile : Tile 
{
    [SerializeField] private float maxStandingTime;
    [SerializeField] private float timeToRebuild;
    BrokenTileState state = BrokenTileState.Idle;
    private float timer = 0f;

    private Collider2D col;
    private Animator animator;

    private void Awake() {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
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

    // use in animation clip
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

    public override void SetTileData(TileData tileData) {
        BrokenTileData brokenTileData = (BrokenTileData)tileData;
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

    public override TileData GetTileData() {
        return new BrokenTileData(state, timer);
    }
}
