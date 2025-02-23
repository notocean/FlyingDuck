using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DuckDir {
    Left = 1, Right = -1
}

[System.Serializable]
public struct DuckData {
    public Vector2 pos;
    public DuckDir viewDir;
    public Vector2 velocity;
    public float energy;

    public DuckData(Vector2 pos, DuckDir viewDir, Vector2 velocity, float energy) {
        this.pos = pos;
        this.viewDir = viewDir;
        this.velocity = velocity;
        this.energy = energy;
    }
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public bool isSave = false;
    public DuckData duckData = new DuckData(Vector2.zero, DuckDir.Left, Vector2.zero, 0);
    public SerializableDictionary<string, TileDataWrapper> tileData = new SerializableDictionary<string, TileDataWrapper>();
    public SerializableDictionary<string, AnimalDataWrapper> animalData = new SerializableDictionary<string, AnimalDataWrapper>();
}
