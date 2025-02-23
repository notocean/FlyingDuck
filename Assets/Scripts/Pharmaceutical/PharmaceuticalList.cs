using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PharmaceuticalList", menuName = "Pharmaceutical/PharmaceuticalList")]
public class PharmaceuticalList : DataManager {
    private static PharmaceuticalList _instance;
    public static PharmaceuticalList Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<PharmaceuticalList>("PharmaceuticalList");
            }
            return _instance;
        }
    }

    const string filename = "PharmaceuticalData";

    public List<Pharmaceutical> pharmaceuticals;
    [HideInInspector] public int currentPharmaceuticalIndex = 0;

    [HideInInspector] public UnityEvent<int> pharmaceuticalChanged = new UnityEvent<int>();

    [HideInInspector] public UnityEvent<int, float> usePharmaceuticalEvent = new UnityEvent<int, float>();
    [HideInInspector] public UnityEvent<int> isActiveEvent = new UnityEvent<int>();

    public void SetCurrentPharmaceutical(int index) {
        currentPharmaceuticalIndex = index;
        pharmaceuticalChanged.Invoke(index);
    }

    public Pharmaceutical GetCurrentPharmaceutical() {
        return pharmaceuticals[currentPharmaceuticalIndex];
    }

    public void UsePharmaceutical() {
        if (pharmaceuticals[currentPharmaceuticalIndex].timeRemaining == 0) {
            pharmaceuticals[currentPharmaceuticalIndex].timeRemaining = pharmaceuticals[currentPharmaceuticalIndex].effectTime;
            pharmaceuticals[currentPharmaceuticalIndex].count--;

            pharmaceuticalChanged.Invoke(currentPharmaceuticalIndex);
            usePharmaceuticalEvent.Invoke(currentPharmaceuticalIndex, pharmaceuticals[currentPharmaceuticalIndex].effectTime);
        }
        else {

        }
    }

    public void FinishUsePharmaceutical(int index) {
        pharmaceuticals[index].timeRemaining = 0;
    }

    public void Buy() {
        PlayerData.Instance.Food -= pharmaceuticals[currentPharmaceuticalIndex].price;
        pharmaceuticals[currentPharmaceuticalIndex].count++;
        pharmaceuticalChanged.Invoke(currentPharmaceuticalIndex);
    }

    public override void Save() {
        List<bool> activeList = new List<bool>();
        List<int> countList = new List<int>();
        List<float> timeRemainingList = new List<float>();
        for (int i = 0; i < pharmaceuticals.Count; i++) {
            activeList.Add(pharmaceuticals[i].isActive);
            countList.Add(pharmaceuticals[i].count);
            timeRemainingList.Add(pharmaceuticals[i].timeRemaining);
        }

        SaveLoadManager.Save(new PharmaceuticalData(activeList, countList, timeRemainingList), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is PharmaceuticalData pharmaceuticalData) {
                List<int> countList = pharmaceuticalData.countList;
                List<bool> activeList = pharmaceuticalData.activeList;
                List<float> timeRemainingList = pharmaceuticalData.timeRemainingList;

                for (int i = 0; i < countList.Count; i++) {
                    pharmaceuticals[i].count = countList[i];
                    pharmaceuticals[i].isActive = activeList[i];
                    pharmaceuticals[i].timeRemaining = timeRemainingList[i];
                }
            }
        }
    }

    private void OnEnable() {
        Load();
    }

    private void OnValidate() {
        if (pharmaceuticals != null) {
            for (int i = 0; i < pharmaceuticals.Count; i++) {
                if (pharmaceuticals[i].index != i) {
                    pharmaceuticals[i].index = i;
                }
            }
        }
    }
}

public class PharmaceuticalData : Data {
    public List<bool> activeList { get; }
    public List<int> countList { get; }
    public List<float> timeRemainingList { get; }

    public PharmaceuticalData(List<bool> activeList, List<int> countList, List<float> timeRemainingList) {
        this.activeList = activeList;
        this.countList = countList;
        this.timeRemainingList = timeRemainingList;
    }
}
