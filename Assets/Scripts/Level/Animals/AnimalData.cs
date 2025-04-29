using UnityEngine;

public class WormData : ObjectData {
    public Vector2 pos { get; set; }
    public int moveDir { get; set; }
    public int posIndex { get; set; }
    public float timer { get; set; }

    public WormData(Vector2 pos, int moveDir, int posIndex, float timer) {
        this.pos = pos;
        this.moveDir = moveDir;
        this.posIndex = posIndex;
        this.timer = timer;
    }
}

public class FrogData : ObjectData {
    public float elapsedTime { get; set; }
    public int startJumpPosIndex { get; set; }
    public int targetJumpPosIndex { get; set; }
    public float timer { get; set; }

    public FrogData(float elapsedTime, int startJumpPosIndex, int targetJumpPosIndex, float timer) {
        this.elapsedTime = elapsedTime;
        this.startJumpPosIndex = startJumpPosIndex;
        this.targetJumpPosIndex = targetJumpPosIndex;
        this.timer = timer;
    }
}

public class ButterflyData : ObjectData {
    public Vector2 pos { get; set; }
    public Vector2 dirMove { get; set; }
    public float refreshTimer { get; set; }

    public ButterflyData(Vector2 pos, Vector2 dirMove, float refreshTimer) {
        this.pos = pos;
        this.dirMove = dirMove;
        this.refreshTimer = refreshTimer;
    }
}