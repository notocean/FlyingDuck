using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HairOutfitManager", menuName = "Duck/HairOutfitManager")]
public class HairOutfitManager : DataManager, IHasAttention {
    private static HairOutfitManager _instance;
    public static HairOutfitManager Instance {
        get {
            if ( _instance == null ) {
                _instance = Resources.Load<HairOutfitManager>("HairOutfitManager");
            }
            return _instance;
        }
    }

    const string filename = "HairActive";

    public List<HairOutfit> hairOutfits;
    public int currentHairIndex;

    public Action hairOutfitChanged;
    public event Action hasAttentionChanged;

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
        hairOutfitChanged?.Invoke();
    }

    public void SetActive(int index) {
        hairOutfits[index].isActive = true;
        hairOutfits[index].hasAttention = true;
    }

    public bool HasAttention() {
        bool hasAttention = false;
        foreach (HairOutfit hair in hairOutfits) {
            if (hair.hasAttention) {
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
        else Save();
    }

    [ExecuteInEditMode]
    public void ResetData(int currentHairIndex, int maxActiveHairIndex) {
        this.currentHairIndex = currentHairIndex;

        for (int i = 0; i < hairOutfits.Count; i++) {
            if (i <= maxActiveHairIndex) {
                hairOutfits[i].isActive = true;
            }
            else {
                hairOutfits[i].isActive = false;
            }
        }
        Save();
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
        
        bool isValidCurrentHairIndex = false;
        foreach (HairOutfit hairOutfit in hairOutfits) {
            if (currentHairIndex == hairOutfit.index) {
                if (hairOutfit.isActive) {
                    isValidCurrentHairIndex = true;
                }
                else isValidCurrentHairIndex = false;
            }
        }
        if (!isValidCurrentHairIndex) {
            currentHairIndex = 0;
        }

        Save();
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