using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HairOutfitList", menuName = "Duck/HairOutfitList")]
public class HairOutfitList : DataManager {
    private static HairOutfitList _instance;
    public static HairOutfitList Instance {
        get {
            if ( _instance == null ) {
                _instance = Resources.Load<HairOutfitList>("HairOutfitList");
            }
            return _instance;
        }
    }

    const string filename = "HairActive";

    public List<HairOutfit> hairOutfits;
    public int currentHairIndex;

    [HideInInspector] public UnityEvent hairOutfitChanged = new UnityEvent();

    public Sprite GetCurrentHair() {
        if (!hairOutfits[currentHairIndex].isActive) {
            currentHairIndex--;
            if (currentHairIndex < 0) {
                currentHairIndex = 0;
                hairOutfits[currentHairIndex].isActive = true;
                return hairOutfits[currentHairIndex].sprite;
            }
            else return GetCurrentHair();
        }
        return hairOutfits[currentHairIndex].sprite;
    }

    public void SetOutfit(int index) {
        currentHairIndex = index;
        hairOutfitChanged.Invoke();
    }

    [HideInInspector] public UnityEvent<int> hairOutfitActived = new UnityEvent<int>();
    public void SetActive(int index) {
        hairOutfits[index].isActive = true;
        hairOutfitActived.Invoke(index);
    }

    public override void Save() {
        List<bool> isActiveList = new List<bool>();
        for (int i = 0; i < hairOutfits.Count; i++) {
            isActiveList.Add(hairOutfits[i].isActive);
        }
        SaveLoadManager.Save(new HairOutfitData(currentHairIndex, isActiveList), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is HairOutfitData hairOutfitData) {
                currentHairIndex = hairOutfitData.currentHairIndex;

                List<bool> isActiveList = hairOutfitData.isActiveList;
                for (int i = 0; i < isActiveList.Count; i++) {
                    hairOutfits[i].isActive = isActiveList[i];
                }
            }
        }
    }

    private void OnEnable() {
        Load();
    }

    private void OnValidate() {
        if (hairOutfits != null) {
            for (int i = 0; i < hairOutfits.Count; i++) {
                if (hairOutfits[i].index != i) {
                    hairOutfits[i].index = i;
                }
            }
        }
    }
}

public class HairOutfitData : Data {
    public int currentHairIndex { get; }
    public List<bool> isActiveList { get; }

    public HairOutfitData(int currentHairIndex, List<bool> isActiveList) {
        this.currentHairIndex = currentHairIndex;
        this.isActiveList = isActiveList;
    }
}