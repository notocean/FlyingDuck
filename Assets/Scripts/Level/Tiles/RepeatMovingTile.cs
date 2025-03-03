using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

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
public class RepeatMovingTile : Tile
{
    [SerializeField] List<MovingPoint> points;
    int pointIndex = 0;
    float timer = 0f;

    Rigidbody2D rb2d;
    Vector2 startPos, endPos;
    float movingTime;

    Vector2 velocity = Vector2.zero;
    PlayerController player;

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
        if (startPos != endPos) {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, timer / movingTime);
            rb2d.MovePosition(newPos);
        }

        if (timer > movingTime) {
            timer = 0f;
            if (pointIndex == points.Count - 1)
                pointIndex = 0;
            else pointIndex++;
        }
    }

    private void InitAMove() {
        startPos = points[pointIndex].pos;
        endPos = pointIndex == points.Count - 1 ? points[0].pos : points[pointIndex + 1].pos;
        movingTime = points[pointIndex].movingTime;

        velocity = (endPos - startPos) / movingTime + Vector2.down;

        if (player != null) {
            player.RemoveVelocityModifier(name);
            player.AddVelocityModifier(name, velocity);
        }
    }

    public override void SetTileData(TileData tileData) {
        RepeatMovingTileData repeatMovingTileData = (RepeatMovingTileData)tileData;
        pointIndex = repeatMovingTileData.pointIndex;
        timer = repeatMovingTileData.timer;
        InitAMove();
        rb2d.MovePosition(Vector2.Lerp(startPos, endPos, timer / movingTime));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot")) {
            player = collision.GetComponentInParent<PlayerController>();
            if (player != null) {
                player.AddVelocityModifier(name, velocity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("PlayerFoot")) {
            if (player != null) {
                player.RemoveVelocityModifier(name);
                player = null;
            }
        }
    }

    public override TileData GetTileData() {
        return new RepeatMovingTileData(pointIndex, timer);
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