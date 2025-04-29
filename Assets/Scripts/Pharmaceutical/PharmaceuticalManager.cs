using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PharmaceuticalManager", menuName = "Pharmaceutical/PharmaceuticalManager")]
public class PharmaceuticalManager : DataManager, IHasAttention {
    private static PharmaceuticalManager _instance;
    public static PharmaceuticalManager Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<PharmaceuticalManager>("PharmaceuticalManager");
            }
            return _instance;
        }
    }

    const string filename = "PharmaceuticalGeneralData";

    public List<Pharmaceutical> pharmaceuticalList;
    public int currentPharmaceuticalIndex = -1;

    public Action<int> pharmaceuticalChanged;
    public event Action hasAttentionChanged;

    public void SetCurrentPharmaceutical(int index) {
        currentPharmaceuticalIndex = index;
        pharmaceuticalChanged?.Invoke(index);
    }

    public Pharmaceutical GetCurrentPharmaceutical() {
        if (currentPharmaceuticalIndex < 0) {
            currentPharmaceuticalIndex = -1;
            return null;
        }
        return pharmaceuticalList[currentPharmaceuticalIndex];
    }

    public void SetActive(int index) {
        pharmaceuticalList[index].isActive = true;
        pharmaceuticalList[index].hasAttention = true;
    }

    public void UsePharmaceutical(IEffectHandler effectHandler) {
        Pharmaceutical pharmaceutical = pharmaceuticalList[currentPharmaceuticalIndex];
        int levelIndex = GameManager.Instance.LevelIndex - 1;

        if (pharmaceutical.timeRemainingList[levelIndex] == 0) {
            pharmaceutical.timeRemainingList[levelIndex] = pharmaceutical.effectTime;
            pharmaceutical.count--;

            // tac dong vao nhan vat
            effectHandler.AddEffect(pharmaceutical);
        }
        else {
            effectHandler.AddEffect(pharmaceutical);
        }
        pharmaceuticalChanged?.Invoke(currentPharmaceuticalIndex);
    }

    public void Buy() {
        PlayerDataManager.Instance.Food -= pharmaceuticalList[currentPharmaceuticalIndex].price;
        pharmaceuticalList[currentPharmaceuticalIndex].count++;
        pharmaceuticalChanged?.Invoke(currentPharmaceuticalIndex);
    }

    public void RefreshPharmaceutical(int index) {
        if (index < 0) return;

        pharmaceuticalChanged?.Invoke(index);
    }

    public bool HasAttention() {
        bool hasAttention = false;
        foreach (Pharmaceutical pharmaceutical in pharmaceuticalList) {
            if (pharmaceutical.hasAttention) {
                hasAttention = true;
                break;
            }
        }

        return hasAttention;
    }

    public void UpdateAttention() {
        hasAttentionChanged?.Invoke();
    }

    public override void Save() {
        List<bool> activeList = new List<bool>();
        List<int> countList = new List<int>();
        List<List<float>> timeRemainingLevelList = new List<List<float>>();

        for (int i = 0; i < pharmaceuticalList.Count; i++) {
            activeList.Add(pharmaceuticalList[i].isActive);
            countList.Add(pharmaceuticalList[i].count);
            timeRemainingLevelList.Add(pharmaceuticalList[i].timeRemainingList);
        }

        SaveLoadManager.Save(new PharmaceuticalGeneralData(activeList, countList, timeRemainingLevelList), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is PharmaceuticalGeneralData pharmaceuticalGeneralData) {
                List<int> countList = pharmaceuticalGeneralData.countList;
                List<bool> activeList = pharmaceuticalGeneralData.activeList;
                List<List<float>> timeRemainingLevelList = pharmaceuticalGeneralData.timeRemainingLevelList;

                for (int i = 0; i < countList.Count; i++) {
                    pharmaceuticalList[i].count = countList[i];
                    pharmaceuticalList[i].isActive = activeList[i];
                    pharmaceuticalList[i].timeRemainingList = timeRemainingLevelList[i];
                }
            }
        }
        else Save();
    }

    [ExecuteInEditMode]
    public void ResetData(int maxActivePharmaceuticalIndex) {
        for (int i = 0; i < pharmaceuticalList.Count; i++) {
            if (i <= maxActivePharmaceuticalIndex) {
                pharmaceuticalList[i].isActive = true;
            }
            else pharmaceuticalList[i].isActive = false;

            pharmaceuticalList[i].count = 0;
            int levelCount = SceneManager.sceneCountInBuildSettings;
            pharmaceuticalList[i].timeRemainingList = new();
            for (int j = 0; j < levelCount - 1; j++) {
                pharmaceuticalList[i].timeRemainingList.Add(0);
            }
        }
        Save();
    }

    private void OnEnable() {
        Load();
    }

    private void OnValidate() {
        if (pharmaceuticalList != null) {
            for (int i = 0; i < pharmaceuticalList.Count; i++) {
                if (pharmaceuticalList[i].index != i) {
                    pharmaceuticalList[i].index = i;
                }
            }
        }

        bool isValidCurrentPharmaceuticalIndex = false;
        foreach (Pharmaceutical pharmaceutical in pharmaceuticalList) {
            if (currentPharmaceuticalIndex == pharmaceutical.index) {
                if (pharmaceutical.isActive) {
                    isValidCurrentPharmaceuticalIndex = true;
                }
                else isValidCurrentPharmaceuticalIndex = false;
            }
        }
        if (!isValidCurrentPharmaceuticalIndex) {
            currentPharmaceuticalIndex = -1;
        }

        Save();
    }
}

public class PharmaceuticalGeneralData : Data {
    public List<bool> activeList { get; }
    public List<int> countList { get; }
    public List<List<float>> timeRemainingLevelList { get; }

    public PharmaceuticalGeneralData(List<bool> activeList, List<int> countList, List<List<float>> timeRemainingLevelList) {
        this.activeList = activeList;
        this.countList = countList;
        this.timeRemainingLevelList = timeRemainingLevelList;
    }
}