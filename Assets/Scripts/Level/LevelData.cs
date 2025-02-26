using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public bool isSave = false;
    public PlayerData playerData = new PlayerData(Vector2.zero, PlayerMoveDir.Left, Vector2.zero, 0);
    public SerializableDictionary<string, TileDataWrapper> tileData = new SerializableDictionary<string, TileDataWrapper>();
    public SerializableDictionary<string, AnimalDataWrapper> animalData = new SerializableDictionary<string, AnimalDataWrapper>();
}
