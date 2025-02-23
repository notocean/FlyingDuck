using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData { }

[Serializable]
public class TileDataWrapper {
    [SerializeReference]
    public TileData data;

    public TileDataWrapper(TileData data) {
        this.data = data;
    }
}

public class RepeatMovingTileData : TileData {
    public int pointIndex { get; set; }
    public float timer { get; set; }

    public RepeatMovingTileData(int pointIndex, float timer) {
        this.pointIndex = pointIndex;
        this.timer = timer;
    }
}

public class BrokenTileData :  TileData {
    public BrokenTileState state { get; set; }
    public float timer { get; set; }

    public BrokenTileData(BrokenTileState state, float timer) {
        this.state = state;
        this.timer = timer;
    }
}