using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Animal, ISaveableObject {
    [SerializeField] protected float heightJump;
    [SerializeField] protected float durationJump;
    [SerializeField] protected float detectDistance;
    [SerializeField] protected float waitToJump;
    [SerializeField] protected int maxPosIndex;             // Chỉ số tối đa của nền tảng mà đối tượng có thể xuât hiện

    float elapsedTime = 0f;
    int startPosIndex = -1;
    int targetPosIndex;
    bool isDetecting = false;

    Transform player;
    AudioPlayer audioPlayer;

    protected override void Awake() {
        base.Awake();
        audioPlayer = GetComponent<AudioPlayer>();
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    protected virtual void Start() {
        player = GameManager.Instance.Player.transform;

        if (startPosIndex == -1) {
            Transform parent = gameObject.transform.parent;
            if (parent == null) {
                Destroy(gameObject);
            }
            startPosIndex = TileManager.Instance.GetPosIndexByName(parent.name);
            transform.position = TileManager.Instance.TileTransformList[startPosIndex].position;
            TileManager.Instance.RegisterPos(startPosIndex);
        }
    }

    protected virtual void Update() {
        if (refreshTimer > 0) {
            refreshTimer -= Time.deltaTime;

            if (refreshTimer <= 0) {
                Spawn();
            }
            return;
        }

        if (!isVisible) return;

        if (!isDetecting) {
            if (Vector2.Distance(transform.position, player.position) <= detectDistance) {
                isDetecting = true;
                StartCoroutine(PrepareJump());
            }
        }
    }

    IEnumerator PrepareJump() {
        yield return new WaitForSeconds(waitToJump);

        SetNewTileIndex(startPosIndex, ref targetPosIndex);

        audioPlayer.Play();
        StartCoroutine(Jump());
    }

    IEnumerator Jump() {
        GetComponent<Collider2D>().enabled = false;

        bool upJump = true;
        if (elapsedTime / durationJump < 0.8f)
            animator.SetTrigger("Jump");
        Vector2 startJumpPos = TileManager.Instance.TileTransformList[startPosIndex].position;
        Vector2 targetJumpPos = TileManager.Instance.TileTransformList[targetPosIndex].position;

        int viewDir = targetJumpPos.x > startJumpPos.x ? -1 : 1;
        transform.localScale = new Vector3(viewDir, 1, 1);

        while (elapsedTime <= durationJump) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationJump;

            if (upJump) {
                if (t >= 0.8f) {
                    upJump = false;
                    animator.SetTrigger("End Jump");
                }
            }

            Vector2 horizontalPosition = Vector2.Lerp(startJumpPos, targetJumpPos, t);
            float verticalOffset = Mathf.Sin(t * Mathf.PI) * heightJump;
            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + verticalOffset);

            yield return null;
        }

        transform.position = targetJumpPos;
        isDetecting = false;
        startPosIndex = targetPosIndex;
        transform.SetParent(TileManager.Instance.TileTransformList[startPosIndex]);
        GetComponent<Collider2D>().enabled = true;
        elapsedTime = 0f;
    }

    void SetNewTileIndex(int currentIndex, ref int targetIndex) {
        targetIndex = TileManager.Instance.GetNewTileIndex(currentIndex, 2, 2, maxPosIndex);
        TileManager.Instance.UnregisterPos(currentIndex);
        TileManager.Instance.RegisterPos(targetIndex);
    }

    protected override void Spawn() {
        base.Spawn();

        SetNewTileIndex(startPosIndex, ref startPosIndex);
        TileManager.Instance.RegisterPos(startPosIndex);
        transform.position = TileManager.Instance.TileTransformList[startPosIndex].position;

        transform.SetParent(TileManager.Instance.TileTransformList[startPosIndex]);
    }

    protected override void Destroy() {
        base.Destroy();

        isDetecting = false;
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }

    public override void SetVisible(bool isVisible) {
        if (this.isVisible != isVisible) {
            this.isVisible = isVisible;
        }
        if (isVisible && !alive) {
            SetLocalVisible(false);
        }
    }

    public ObjectData GetObjectData() {
        return new FrogData(elapsedTime, startPosIndex, targetPosIndex, refreshTimer);
    }

    public void SetObjectData(ObjectData data) {
        TileManager.Instance.UnregisterPos(startPosIndex);

        FrogData _data = data as FrogData;
        elapsedTime = _data.elapsedTime;
        startPosIndex = _data.startJumpPosIndex;
        targetPosIndex = _data.targetJumpPosIndex;

        if (_data.timer <= 0) {
            base.Spawn();
            if (elapsedTime > 0f) {
                TileManager.Instance.UnregisterPos(startPosIndex);
                TileManager.Instance.RegisterPos(targetPosIndex);
                StartCoroutine(Jump());
            }
            else {
                TileManager.Instance.RegisterPos(startPosIndex);
                transform.position = TileManager.Instance.TileTransformList[startPosIndex].position;
                isDetecting = false;
            }
        }
        else Destroy();

        refreshTimer = _data.timer;
    }
}
