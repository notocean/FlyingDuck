using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Animal, ITeleportable, ISaveableObject {
    [SerializeField] protected MapSideTeleportEvent mapSideTeleportEvent;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float limitRotateAngle;
    [SerializeField] protected float timeToRotateDirMove;
    [SerializeField] protected float delayToStop;

    [SerializeField] DetectObjectsButterfly detectObjectsButterfly;
    [SerializeField] LimitArea limitVertical;

    Vector2 dirMove = Vector2.up;

    HashSet<Collider2D> detectedObjects = new HashSet<Collider2D>();

    protected override void Awake() {
        base.Awake();
        LevelDataManager.Instance.RegisterSaveableObject(name, this);
    }

    protected virtual void Start() {
        mapSideTeleportEvent.RaiseRegisterEvent(transform);
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

        transform.position = transform.position + (Vector3)(moveSpeed * dirMove * Time.deltaTime);

        Vector2 addDirection = Vector2.zero;
        if (transform.position.y > limitVertical.max) addDirection.y = -1f;
        else if (transform.position.y < limitVertical.min) addDirection.y = 1f;

        if (addDirection != Vector2.zero) {
            ChangeDirMove(addDirection);
            DisplayVisual();
        }
    }

    void ChangeDirMove(Vector2 addDirection) {
        if (CustomMathf.IsOpposite(dirMove, addDirection)) {
            addDirection = new Vector2(addDirection.y, -addDirection.x);
        }
        dirMove = Vector2.Lerp(dirMove, dirMove + addDirection, 4 * Time.deltaTime).normalized;
    }

    void HandleDetectObjects(Collider2D collider, bool isEnter) {
        if (isEnter) {
            if (!detectedObjects.Contains(collider)) {
                detectedObjects.Add(collider);
            }
        }
        else if (detectedObjects.Contains(collider)) {
            detectedObjects.Remove(collider);
        }
        CalculateDirMove();
    }

    void CalculateDirMove() {
        Vector2 addDirection = dirMove;
        foreach (Collider2D collider in detectedObjects) {
            Vector2 objToThis = transform.position - collider.bounds.center;
            addDirection = (addDirection + objToThis).normalized;
        }

        ChangeDirMove(addDirection);
        DisplayVisual();
    }

    protected virtual void DisplayVisual() {
        transform.localScale = new Vector3(dirMove.x > 0 ? -1 : 1, 1, 1);

        // Góc giữa hướng hiện tại và Vecter up
        float angle = Vector2.Angle(dirMove, Vector2.up);
        bool isNegative = false;

        if (angle <= 90) {
            if (dirMove.x <= 0) {
                isNegative = true;
            }
            angle = 90 - angle;
        }
        else {
            if (dirMove.x > 0) isNegative = true;
            angle = angle - 90;
        }


        float rotateAngle = limitRotateAngle * angle / 90 * (isNegative ? -1 : 1);

        transform.rotation = Quaternion.Euler(0, 0, rotateAngle);
    }

    protected override void Destroy() {
        base.Destroy();
        refreshTimer = 0f;
        GetComponent<Collider2D>().enabled = true;
        StartCoroutine(WaitToStop());
    }

    IEnumerator WaitToStop() {
        yield return new WaitForSeconds(delayToStop);
        refreshTimer = timeToRefresh;
        GetComponent<Collider2D>().enabled = false;
    }

    protected virtual void OnEnable() {
        detectObjectsButterfly.OnTrigger += HandleDetectObjects;
    }

    protected virtual void OnDisable() {
        detectObjectsButterfly.OnTrigger -= HandleDetectObjects;
    }

    public void Teleport(Vector2 newPos) {
        transform.position = newPos;
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)dirMove);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(GameSettings.Instance.minHorizontalWorld, limitVertical.min), new Vector2(GameSettings.Instance.maxHorizontalWorld, limitVertical.min));
        Gizmos.DrawLine(new Vector2(GameSettings.Instance.minHorizontalWorld, limitVertical.max), new Vector2(GameSettings.Instance.maxHorizontalWorld, limitVertical.max));
    }

    public override void SetVisible(bool isVisible) {
        if (this.isVisible != isVisible) {
            this.isVisible = isVisible;
            if (isVisible) {
                mapSideTeleportEvent.RaiseRegisterEvent(transform);
            }
            else {
                mapSideTeleportEvent.RaiseUnregisterEvent(transform);
            }
        }
        if (isVisible && !alive) {
            SetLocalVisible(false);
        }
    }

    public ObjectData GetObjectData() {
        return new ButterflyData(transform.position, dirMove, refreshTimer);
    }

    public void SetObjectData(ObjectData data) {
        ButterflyData _data = data as ButterflyData;
        transform.position = _data.pos;
        dirMove = _data.dirMove;

        if (_data.refreshTimer <= 0) {
            Spawn();
        }
        else Destroy();

        refreshTimer = _data.refreshTimer;
    }
}

[Serializable]
public class LimitArea {
    public float min;
    public float max;
}