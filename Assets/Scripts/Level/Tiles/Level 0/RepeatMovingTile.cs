using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MovingPoint {
    public Vector2 pos;
    public float movingTime;

    public MovingPoint(Vector2 pos, float movingTime) {
        this.pos = pos;
        this.movingTime = movingTime;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RepeatMovingTile : MonoBehaviour, ISaveableObject
{
    [SerializeField] List<MovingPoint> points;
    int pointIndex = 0;
    float timer = 0f;

    Rigidbody2D rb2d;
    Vector2 startPos, endPos;
    float movingTime;

    public Action OnVelocityChanged;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        if (points.Count == 0) {
            points.Add(new MovingPoint(transform.position, 100f));
            points.Add(new MovingPoint(transform.position, 100f));
        }
    }

    private void FixedUpdate() {
        if (timer == 0f)
            InitAMove();

        timer += Time.fixedDeltaTime;

        if (timer > movingTime) {
            timer = 0f;
            pointIndex = (pointIndex + 1) % points.Count;
        }
    }

    void InitAMove() {
        startPos = points[pointIndex].pos;
        endPos = points[(pointIndex + 1) % points.Count].pos;
        movingTime = points[pointIndex].movingTime;

        rb2d.MovePosition(startPos);
        rb2d.velocity = (endPos - startPos) / movingTime;
        OnVelocityChanged?.Invoke();
    }

    //private void FixedUpdate() {
    //    if (timer == 0f)
    //        InitAMove();

    //    timer += Time.fixedDeltaTime;
    //    if (startPos != endPos) {
    //        Vector2 newPos = Vector2.Lerp(startPos, endPos, timer / movingTime);
    //        rb2d.MovePosition(newPos);
    //    }

    //    if (timer > movingTime) {
    //        timer = 0f;
    //        if (pointIndex == points.Count - 1)
    //            pointIndex = 0;
    //        else pointIndex++;
    //    }
    //}

    //private void InitAMove() {
    //    startPos = points[pointIndex].pos;
    //    endPos = pointIndex == points.Count - 1 ? points[0].pos : points[pointIndex + 1].pos;
    //    movingTime = points[pointIndex].movingTime;

    //    velocity = (endPos - startPos) / movingTime + Vector2.down;

    //    if (player != null) {
    //        player.RemoveVelocityModifier(name);
    //        player.AddVelocityModifier(name, velocity);
    //    }
    //}

    public ObjectData GetObjectData() {
        return new RepeatMovingTileData(pointIndex, timer);
    }

    public void SetObjectData(ObjectData data) {
        RepeatMovingTileData repeatMovingTileData = (RepeatMovingTileData)data;
        pointIndex = repeatMovingTileData.pointIndex;
        timer = repeatMovingTileData.timer;
        InitAMove();

        if (rb2d == null) {
            rb2d = GetComponent<Rigidbody2D>();
        }
        rb2d.MovePosition(Vector2.Lerp(startPos, endPos, timer / movingTime));
    }

    [ExecuteInEditMode]
    public void AddPoint() {
        points.Add(new MovingPoint(transform.position, 0f));
    }

    [ExecuteInEditMode]
    public void UpdatePosition(int index) {
        if (index >= 0 && index < points.Count)
            transform.position = points[index].pos;
    }
}

public class RepeatMovingTileData : ObjectData {
    public int pointIndex { get; }
    public float timer { get; }

    public RepeatMovingTileData(int pointIndex, float timer) {
        this.pointIndex = pointIndex;
        this.timer = timer;
    }

    public override ObjectData Clone() {
        return new RepeatMovingTileData(pointIndex, timer);
    }
}