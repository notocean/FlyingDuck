using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Worm : Animal {
    [SerializeField] WormConfigs config;

    float timer;
    [SerializeField] int moveDir = 1;
    bool isMoving = false;
    bool canMove = true;
    bool alive = true;

    Animator animator;
    RaycastHit2D hit;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(Move());
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;

            if (timer <= 0) {
                Spawn();
            }
            return;
        }

        hit = Physics2D.Raycast(transform.position + new Vector3(-transform.localScale.x * config.offset.x, config.offset.y), new Vector3(config.forwardDetect.x * transform.localScale.x, config.forwardDetect.y), config.forwardDetect.magnitude, config.layerMask);

        if (hit.collider != null) {
            if ((config.obstacleLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0 && isMoving) {
                canMove = false;
            }
        }
        else canMove = false;
    }

    IEnumerator Move() {
        isMoving = true;
        canMove = true;
        moveDir *= -1;
        transform.localScale = new Vector3(moveDir, 1, 1);
        animator.SetBool("Is Idle", false);

        while (canMove) {
            Vector2 pos = transform.localPosition + new Vector3(-moveDir * config.moveSpeed * Time.deltaTime, 0);
            transform.localPosition = pos;
            yield return null;
        }

        animator.SetBool("Is Idle", true);
        isMoving = false;
        yield return new WaitForSecondsRealtime(config.waitToMoveTime);

        StartCoroutine(Move());
    }

    public override AnimalData GetAnimalData() {
        return new WormData(transform.localPosition, moveDir, timer);
    }

    public override void SetAnimalData(AnimalData animalData) {
        WormData wormData = (WormData)animalData;
        transform.localPosition = wormData.pos;
        moveDir = wormData.moveDir;
        timer = wormData.timer;

        if (timer <= 0)
            Spawn();
        else IsDeleted();
        timer = wormData.timer;
    }

    public override int Collected() {
        if (alive) {
            IsDeleted();

            GameObject showCollectedDialog = Instantiate(config.showCollectedDialogPrefab, transform.position, Quaternion.identity);
            if (showCollectedDialog != null) {
                TMP_Text foodText = showCollectedDialog.GetComponentInChildren<TMP_Text>();
                if (foodText != null) {
                    foodText.text = $"+{config.food}";
                }
            }

            return config.food;
        }
        else return 0;
    }

    private void Spawn() {
        alive = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        StopAllCoroutines();
        StartCoroutine(Move());
    }

    private void IsDeleted() {
        alive = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        timer = config.timeToRefresh;
        StopAllCoroutines();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + new Vector3(-transform.localScale.x * config.offset.x, config.offset.y), transform.position + new Vector3(-transform.localScale.x * config.offset.x, config.offset.y) + new Vector3(config.forwardDetect.x * transform.localScale.x, config.forwardDetect.y));
    }
}
