using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : DataManager {
    private static PlayerData _instance;
    public static PlayerData Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<PlayerData>("PlayerData");
            }
            return _instance;
        }
    }

    const string filename = "PlayerData";

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
            //Save
        }
    }

    [ExecuteAlways]
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
    }

    private void OnEnable() {
        Load();
    }
}

[Serializable]
public class PlayerJsonData : Data {
    public int food { get; }

    public PlayerJsonData(int food) {
        this.food = food;
    }
}
