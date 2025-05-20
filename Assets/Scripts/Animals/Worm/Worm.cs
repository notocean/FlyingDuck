using UnityEngine;

public class Worm : Animal, ISaveableObject {
    [SerializeField] protected Vector2 forwardDetect;
    [SerializeField] protected Vector2 detectOffset;
    [SerializeField] protected LayerMask detectLayerMask;
    [SerializeField] protected LayerMask obstacleLayerMask;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float waitToMoveTime;
    [SerializeField] protected int maxPosIndex;
    [SerializeField] protected Vector2 posOffset;

    int moveDir;
    int MoveDir {
        get { return moveDir; }
        set {
            moveDir = value;
            transform.localScale = new Vector3(moveDir, 1, 1);
        }
    }

    bool isMoving = false;
    bool canMove = true;
    float waitToMoveTimer = 0f;
    int posIndex = -1;

    RaycastHit2D hit;

    protected override void Awake() {
        base.Awake();
        MoveDir = 1;
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    protected virtual void Start() {
        if (posIndex == -1) {
            Transform parent = gameObject.transform.parent;
            if (parent == null) {
                Destroy(gameObject);
            }
            posIndex = TileManager.Instance.GetPosIndexByName(parent.name);
            transform.position = TileManager.Instance.TileTransformList[posIndex].position + (Vector3)posOffset;
            TileManager.Instance.RegisterPos(posIndex);
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

        hit = Physics2D.Raycast(transform.position + new Vector3(-MoveDir * detectOffset.x, detectOffset.y), new Vector3(forwardDetect.x * MoveDir, forwardDetect.y), forwardDetect.magnitude, detectLayerMask);

        if (hit.collider != null) {
            if ((obstacleLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0 && isMoving) {
                canMove = false;
            }
        }
        else canMove = false;

        if (isMoving) {
            if (canMove) {
                Vector2 pos = transform.localPosition + new Vector3(-MoveDir * moveSpeed * Time.deltaTime, 0);
                transform.localPosition = pos;
            }
            else {
                animator.SetBool("Is Idle", true);
                isMoving = false;
                waitToMoveTimer = waitToMoveTime;
            }
        }
        else if (waitToMoveTimer <= 0) {
            // Khởi tạo bước di chuyển mới
            isMoving = true;
            canMove = true;
            MoveDir *= -1;
            animator.SetBool("Is Idle", false);
        }

        if (waitToMoveTimer > 0) {
            waitToMoveTimer -= Time.deltaTime;
        }
    }

    void SetNewTileIndex(ref int targetIndex) {
        TileManager.Instance.UnregisterPos(targetIndex);
        targetIndex = TileManager.Instance.GetNewTileIndex(targetIndex, 2, 2, maxPosIndex);
        TileManager.Instance.RegisterPos(targetIndex);
    }

    protected override void Spawn() {
        base.Spawn();

        SetNewTileIndex(ref posIndex);
        transform.position = TileManager.Instance.TileTransformList[posIndex].position + (Vector3)posOffset;
        transform.SetParent(TileManager.Instance.TileTransformList[posIndex]);
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + new Vector3(-transform.localScale.x * detectOffset.x, detectOffset.y), transform.position + new Vector3(-transform.localScale.x * detectOffset.x, detectOffset.y) + new Vector3(forwardDetect.x * transform.localScale.x, forwardDetect.y));
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
        return new WormData(transform.localPosition, MoveDir, posIndex, refreshTimer);
    }

    public void SetObjectData(ObjectData data) {
        TileManager.Instance.UnregisterPos(posIndex);

        WormData wormData = (WormData)data;
        MoveDir = wormData.moveDir;
        posIndex = wormData.posIndex;

        if (wormData.timer <= 0) {
            TileManager.Instance.RegisterPos(posIndex);
            base.Spawn();
            transform.SetParent(TileManager.Instance.TileTransformList[posIndex]);
            transform.localPosition = wormData.pos;
        }
        else Destroy();

        refreshTimer = wormData.timer;
    }
}
