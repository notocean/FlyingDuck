using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerDataManager", menuName = "Game/PlayerDataManager")]
public class PlayerDataManager : DataManager {
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<PlayerDataManager>("PlayerDataManager");
            }
            return _instance;
        }
    }

    const string filename = "PlayerDataManager";

    [HideInInspector] public UnityEvent<int> foodChanged = new UnityEvent<int>();

    [SerializeField]
    private int food;
    public int Food {
        get { return food; }
        set {
            if (value < 0)
                food = 0;
            else food = value;
            foodChanged.Invoke(food);
        }
    }

    public override void Save() {
        SaveLoadManager.Save(new PlayerJsonData(food), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is PlayerJsonData playerJsonData) {
                food = playerJsonData.food;
            }
        }
        else Save();
    }

    [ExecuteInEditMode]
    public void ResetData(int food) {
        Food = food;
        Save();
    }

    private void OnEnable() {
        Load();
    }

    private void OnValidate() {
        Save();
    }
}

[Serializable]
public class PlayerJsonData : Data {
    public int food { get; }

    public PlayerJsonData(int food) {
        this.food = food;
    }
}
