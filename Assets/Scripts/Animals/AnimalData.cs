using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalData { }

[Serializable]
public class AnimalDataWrapper {
    [SerializeReference] 
    public AnimalData data;

    public AnimalDataWrapper(AnimalData data) {
        this.data = data;
    }
}


public class WormData : AnimalData {
    public Vector2 pos { get; set; }
    public int moveDir { get; set; }
    public float timer { get; set; }

    public WormData(Vector2 pos, int moveDir, float timer) {
        this.pos = pos;
        this.moveDir = moveDir;
        this.timer = timer;
    }
}