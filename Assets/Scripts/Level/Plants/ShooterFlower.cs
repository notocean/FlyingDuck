using System.Collections;
using UnityEngine;

public class ShooterFlower : MonoBehaviour, ISaveableObject
{
    [SerializeField] Transform rootTrans;
    [SerializeField] Transform targetTrans;
    [SerializeField] Transform shootTrans;

    [SerializeField] float rotateSpeed;
    [SerializeField] Vector2 limitTopArea;
    [SerializeField] float limitBottom;
    [SerializeField] float reloadTime;

    Animator animator;
    Transform player;
    bool isNotIdle;
    bool doMoveIdle;
    bool canShoot = true;
    bool isShooted;
    float reloadTimer;

    Vector2 root2Target;
    float rotateAngle;

    Vector2 boundLeftTop, boundRightTop;

    private void Awake() {
        player = GameManager.Instance.Player.transform;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        boundLeftTop = rootTrans.position + new Vector3(-limitTopArea.x, limitTopArea.y);
        boundRightTop = rootTrans.position + new Vector3(limitTopArea.x, limitTopArea.y);
    }

    private void Update() {
        if (!canShoot) return;

        doMoveIdle = false;

        if (CustomMathf.IsPointInTriangle(player.position, rootTrans.position, boundLeftTop, boundRightTop)
            || CustomMathf.IsPointInRectangle(player.position, boundLeftTop.y, boundLeftTop.y + limitBottom, boundRightTop.x, boundLeftTop.x)) {
            if (player.position.y < rootTrans.position.y) {
                if (!isNotIdle) {
                    isNotIdle = true;
                }

                rotateAngle = Vector2.SignedAngle(Vector2.down, player.position - rootTrans.position);

                root2Target = Quaternion.Euler(0, 0, rotateAngle) * Vector2.down;
                Vector3 targetPosition = rootTrans.position + (Vector3)root2Target;

                targetTrans.position = Vector2.MoveTowards(targetTrans.position, targetPosition, rotateSpeed * Time.deltaTime);

                if (targetTrans.position == targetPosition) {
                    canShoot = false;
                    isShooted = false;
                    StartCoroutine(StartShoot());
                }
            }
            else if (isNotIdle) {
                doMoveIdle = true;
            }
        }
        else if (isNotIdle) {
            doMoveIdle = true;
        }

        if (doMoveIdle) {
            Vector3 targetPosition = rootTrans.position + Vector3.down;
            targetTrans.position = Vector2.MoveTowards(targetTrans.position, targetPosition, rotateSpeed * Time.deltaTime);
            if (targetTrans.position == targetPosition) {
                isNotIdle = false;
            }
        }
    }

    IEnumerator StartShoot() {
        animator.SetTrigger("Shoot");
        yield return new WaitUntil(() => isShooted);

        yield return StartCoroutine(MoveToIdlePos());
    }

    IEnumerator MoveToIdlePos() {
        Vector3 targetPosition = rootTrans.position + Vector3.down;
        while (targetTrans.position != targetPosition) {
            targetTrans.position = Vector2.MoveTowards(targetTrans.position, targetPosition, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        reloadTimer = 0f;
        yield return StartCoroutine(ReLoad());
    }

    IEnumerator ReLoad() {
        while (reloadTimer < reloadTime) {
            reloadTimer += Time.deltaTime;
            yield return null;
        }
        reloadTimer = 0f;
        canShoot = true;
    }

    public void Shoot() {
        GameObject bulletObj = ObjectPoolingManager.Instance.GetObject("ShooterFlowerBullet");
        bulletObj.transform.position = shootTrans.position;
        bulletObj.transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
        ShooterFlowerBullet shooterFlowerBullet = bulletObj.GetComponent<ShooterFlowerBullet>();
        shooterFlowerBullet.Launch(root2Target);
        isShooted = true;
    }

    public ObjectData GetObjectData() {
        return new ShooterFlowerData(targetTrans.position, canShoot, isShooted, reloadTimer);
    }

    public void SetObjectData(ObjectData data) {
        ShooterFlowerData _data = data as ShooterFlowerData;
        targetTrans.position = _data.targetPos;
        canShoot = _data.canShoot;
        isShooted= _data.isShooted;
        reloadTimer = _data.reloadTimer;

        if (!canShoot) {
            if (isShooted) {
                if (reloadTimer != 0) {
                    StartCoroutine(ReLoad());
                }
                else {
                    StartCoroutine(MoveToIdlePos());
                }
            }
            else {
                StartCoroutine(StartShoot());
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rootTrans.position, rootTrans.position + new Vector3(-limitTopArea.x, limitTopArea.y));
        Gizmos.DrawLine(rootTrans.position, rootTrans.position + new Vector3(limitTopArea.x, limitTopArea.y));
        Gizmos.DrawLine(rootTrans.position + new Vector3(-limitTopArea.x, limitTopArea.y), rootTrans.position + new Vector3(-limitTopArea.x, limitTopArea.y) + new Vector3(0, limitBottom));
        Gizmos.DrawLine(rootTrans.position + new Vector3(limitTopArea.x, limitTopArea.y), rootTrans.position + new Vector3(limitTopArea.x, limitTopArea.y) + new Vector3(0, limitBottom));
        Gizmos.DrawLine(rootTrans.position + new Vector3(-limitTopArea.x, limitTopArea.y) + new Vector3(0, limitBottom), rootTrans.position + new Vector3(limitTopArea.x, limitTopArea.y) + new Vector3(0, limitBottom));
    }
}

public class ShooterFlowerData : ObjectData {
    public Vector2 targetPos { get; }
    public bool canShoot { get; }
    public bool isShooted { get; }
    public float reloadTimer { get; }

    public ShooterFlowerData(Vector2 targetPos, bool canShoot, bool isShooted, float reloadTimer) {
        this.targetPos = targetPos;
        this.canShoot = canShoot;
        this.isShooted = isShooted;
        this.reloadTimer = reloadTimer;
    }
}
