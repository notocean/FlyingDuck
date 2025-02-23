using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public abstract void SetTileData(TileData tileData);
    public abstract TileData GetTileData();
}
