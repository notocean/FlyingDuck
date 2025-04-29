using System;
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

    [SerializeField] int maxAvaiableLevelIndex;
    public int MaxAvaiableLevelIndex {
        get { return maxAvaiableLevelIndex; }
        private set { }
    }

    [SerializeField] int levelCount;
    public List<LevelSprite> levelUISprites;
    public List<LevelData> defaultLevelData;
    public List<LevelData> levelData;

    const string levelGeneralFileName = "LevelGeneral";

    [HideInInspector] public UnityEvent levelSelectedChanged = new UnityEvent();

    public void ChangeCurrentLevel(int value) {
        int index = currentLevelIndex + value;
        if (index >= 0 && index < levelCount) {
            currentLevelIndex = index;
            levelSelectedChanged.Invoke();
        }
    }

    public void IncreaseMaxActiveLevel() {
        maxActiveLevelIndex = Mathf.Clamp(maxActiveLevelIndex + 1, 0, MaxAvaiableLevelIndex);
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
        levelData[currentLevelIndex].objectDataWrapper = defaultLevelData[currentLevelIndex].objectDataWrapper;
    }

    public void SaveGeneral() {
        SaveLoadManager.Save(new LevelGeneralData(currentLevelIndex, maxActiveLevelIndex, MaxAvaiableLevelIndex), levelGeneralFileName);
    }

    public void SaveDefaultLevel() {
        SaveDefaultLevel(currentLevelIndex);
    }

    public void SaveDefaultLevel(int i) {
        SaveLoadManager.Save(defaultLevelData[i], $"DefaultLevel{defaultLevelData[i].index}");
    }

    public void SaveLevel() {
        SaveLevel(currentLevelIndex);
    }

    public void SaveLevel(int i) {
        SaveLoadManager.Save(levelData[i], $"Level{levelData[i].index}");
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
                MaxAvaiableLevelIndex = levelGeneralData.maxAvaiableLevelIndex;
            }
        }
        else SaveGeneral();

        for (int i = 0; i < defaultLevelData.Count; i++) {
            Data data = SaveLoadManager.Load($"DefaultLevel{defaultLevelData[i].index}");
            if (data != null) {
                if (data is LevelData defaulData) {
                    defaultLevelData[i].isSaved = defaulData.isSaved;
                    defaultLevelData[i].objectDataWrapper = defaulData.objectDataWrapper;
                }
            }
            else SaveDefaultLevel(i);
        }

        for (int i = 0; i < levelData.Count; i++) {
            Data data = SaveLoadManager.Load($"Level{levelData[i].index}");
            if (data != null) {
                if (data is LevelData _data) {
                    levelData[i].isSaved = _data.isSaved;
                    levelData[i].objectDataWrapper = _data.objectDataWrapper;
                }
            }
            else SaveLevel(i);
        }
    }

    [ExecuteInEditMode]
    public void ResetData(int currentLevelIndex, int maxActiveLevelIndex, int maxAvaiableLevelIndex) {
        this.currentLevelIndex = currentLevelIndex;
        this.maxActiveLevelIndex = maxActiveLevelIndex;
        this.MaxAvaiableLevelIndex = maxAvaiableLevelIndex;
        this.SaveGeneral();

        for (int i = 0; i < defaultLevelData.Count; i++) {
            defaultLevelData[i].isSaved = false;
            defaultLevelData[i].objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();
            SaveDefaultLevel(i);
        }

        for (int i = 0; i < levelData.Count; i++) {
            levelData[i].isSaved = false;
            levelData[i].objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();
            SaveLevel(i);
        }
    }

    private void OnEnable() {
        Load();
    }

    private void OnValidate() {
        maxAvaiableLevelIndex = Mathf.Clamp(maxAvaiableLevelIndex, 0, levelCount - 1);

        ChangeList<LevelSprite>(ref levelUISprites, new LevelSprite(), levelCount);
        ChangeList<LevelData>(ref defaultLevelData, new LevelData(), levelCount);
        ChangeList<LevelData>(ref levelData, new LevelData(), levelCount);

        for (int i = 0; i < levelCount; i++) {
            defaultLevelData[i].index = i;
            defaultLevelData[i].objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();

            levelData[i].index = i;
            levelData[i].objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();
        }
    }

    void ChangeList<T>(ref List<T> list, T item, int targetListCount) {
        while (list.Count > targetListCount) {
            list.RemoveAt(list.Count - 1);
        }

        while (list.Count < targetListCount) {
            list.Add(item);
        }
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