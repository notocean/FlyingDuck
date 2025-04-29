using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ResetData", menuName = "Game/ResetData")]
public class ResetData : ScriptableObject {
    public GameSettings gameSettings;
    public HairOutfitManager hairOutfitManager;
    public LevelManager levelManager;
    public PharmaceuticalManager pharmaceuticalManager;
    public PlayerDataManager playerDataManager;

    [Header("Default Game Settings Data")]
    public float holdPoint;
    public float musicValue;
    public float vfxValue;
    public bool isTutorial;

    [Header("Default Hair Outfit Manager Data")]
    public int currentHairIndex;
    public int maxActiveHairIndex;

    [Header("Default Level Manager Data")]
    public int currentLevelIndex;
    public int maxActiveLevelIndex;
    public int maxAvaiableLevelIndex;

    [Header("Default Pharmaceutical Manager Data")]
    public int maxActivePharmaceuticalIndex;

    [Header("Default Player Data Manager Data")]
    public int food;

    public void Reset() {
        gameSettings.ResetData(holdPoint, musicValue, vfxValue, isTutorial);
        hairOutfitManager.ResetData(currentHairIndex, maxActiveHairIndex);
        levelManager.ResetData(currentLevelIndex, maxActiveLevelIndex, maxAvaiableLevelIndex); ;
        pharmaceuticalManager.ResetData(maxActivePharmaceuticalIndex);
        playerDataManager.ResetData(food);
    }
}
