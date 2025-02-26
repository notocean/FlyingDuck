using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LevelManager", menuName = "Game/LevelManager")]
public class LevelManager : DataManager {
    private static LevelManager _instance;
    public static LevelManager Instance {
        get {
            if ( _instance == null) {
                _instance = Resources.Load<LevelManager>("LevelManager");
            }
            return _instance;
        }
    }

    public int currentLevelIndex;
    public int maxActiveLevelIndex;
    public int maxAvaiableLevelIndex;
    public List<LevelSprite> levelUISprites;
    public List<LevelData> defaultLevelData;
    public List<LevelData> levelData;

    const string levelGeneralFileName = "LevelGeneral";

    [HideInInspector] public UnityEvent levelSelectedChanged = new UnityEvent();

    public void ChangeCurrentLevel(int value) {
        int index = currentLevelIndex + value;
        if (index >= 0 && index < levelUISprites.Count) {
            currentLevelIndex = index;
            levelSelectedChanged.Invoke();
        }
    }

    public void IncreaseMaxActiveLevel() {
        maxActiveLevelIndex = Mathf.Clamp(maxActiveLevelIndex + 1, 0, levelUISprites.Count);
    }

    public LevelSprite GetCurrentLevelSprite() {
        return levelUISprites[currentLevelIndex];
    }

    public bool IsActiveLevel() {
        return currentLevelIndex <= maxActiveLevelIndex;
    }

    public LevelData GetCurrentLevelData() {
        return levelData[currentLevelIndex];
    }

    public LevelData GetDefaultLevelData() {
        return defaultLevelData[currentLevelIndex];
    }

    public void ResetLevel() {
        levelData[currentLevelIndex].playerData = defaultLevelData[currentLevelIndex].playerData;
        levelData[currentLevelIndex].tileData = defaultLevelData[currentLevelIndex].tileData;
        levelData[currentLevelIndex].animalData = defaultLevelData[currentLevelIndex].animalData;
    }

    public void SaveGeneral() {
        SaveLoadManager.Save(new LevelGeneralData(currentLevelIndex, maxActiveLevelIndex, maxAvaiableLevelIndex), levelGeneralFileName);
    }

    public void SaveLevel(int i) {
        SaveLoadManager.Save(new LevelJsonData(levelData[i].isSave, levelData[i].playerData, levelData[i].tileData, levelData[i].animalData), $"{levelData[i].name}");
    }

    public override void Save() {
        SaveGeneral();
        for (int i = 0; i < levelData.Count; i++) {
            SaveLevel(i);
        }
    }

    public override void Load() {
        Data data1 = SaveLoadManager.Load(levelGeneralFileName);
        if (data1 != null) {
            if (data1 is LevelGeneralData levelGeneralData) {
                currentLevelIndex = levelGeneralData.currentLevelIndex;
                maxActiveLevelIndex = levelGeneralData.maxActiveLevelIndex;
                maxAvaiableLevelIndex = levelGeneralData.maxAvaiableLevelIndex;
            }
        }

        for (int i = 0; i < levelData.Count; i++) {
            Data data = SaveLoadManager.Load($"{levelData[i].name}");
            if (data != null) {
                if (data is LevelJsonData levelJsonData) {
                    levelData[i].isSave = levelJsonData.isSave;
                    levelData[i].playerData = levelJsonData.playerData;
                    levelData[i].tileData = levelJsonData.tileData;
                    levelData[i].animalData = levelJsonData.animalData;
                }
            }
        }
    }

    private void OnEnable() {
        Load();
    }
}

[Serializable]
public class LevelGeneralData : Data {
    public int currentLevelIndex { get; }
    public int maxActiveLevelIndex { get; }
    public int maxAvaiableLevelIndex { get; }

    public LevelGeneralData(int currentLevelIndex, int maxActiveLevelIndex, int maxAvaiableLevelIndex) {
        this.currentLevelIndex = currentLevelIndex;
        this.maxActiveLevelIndex = maxActiveLevelIndex;
        this.maxAvaiableLevelIndex = maxAvaiableLevelIndex;
    }
}

public class LevelJsonData : Data {
    public bool isSave { get; }
    public PlayerData playerData { get; }
    public SerializableDictionary<string, TileDataWrapper> tileData { get; }
    public SerializableDictionary<string, AnimalDataWrapper> animalData { get; }

    public LevelJsonData(bool isSave, PlayerData playerData, SerializableDictionary<string, TileDataWrapper> tileData, SerializableDictionary<string, AnimalDataWrapper> animalData) {
        this.isSave = isSave;
        this.playerData = playerData;
        this.tileData = tileData;
        this.animalData = animalData;
    }
}