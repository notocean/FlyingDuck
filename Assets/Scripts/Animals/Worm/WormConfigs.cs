using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WormConfigs", menuName = "Configs/WormConfigs")]
public class WormConfigs : ScriptableObject
{
    public Vector2 forwardDetect;
    public Vector2 offset;
    public LayerMask layerMask;
    public LayerMask obstacleLayerMask;
    public float moveSpeed;
    public float waitToMoveTime;

    public int food;
    public float timeToRefresh;

    public GameObject showCollectedDialogPrefab;
}
