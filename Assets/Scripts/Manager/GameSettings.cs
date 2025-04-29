using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings")]
public class GameSettings : DataManager {
    private static GameSettings _instance;
    public static GameSettings Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<GameSettings>("GameSettings");
            }
            return _instance;
        }
    }

    public float centerOfWorld;
    public float minHorizontalWorld;
    public float maxHorizontalWorld;
    public float mapWidth {
        get { return Mathf.Abs(maxHorizontalWorld - minHorizontalWorld); }
    }

    public float HoldPoint;

    [Range(0.0001f, 1f)]
    public float MusicVolume;

    [Range(0.0001f, 1f)]
    public float SoundFXVolume;

    public bool isTutorial;

    const string filename = "GameSettings";

    public override void Save() {
        SaveLoadManager.Save(new GameSettingsData(MusicVolume, SoundFXVolume, isTutorial), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is GameSettingsData gameSettingsData) {
                MusicVolume = gameSettingsData.musicVolume;
                SoundFXVolume = gameSettingsData.soundFXVolume;
                isTutorial = gameSettingsData.isTutorial;
            }
        }
        else Save();
    }

    [ExecuteInEditMode]
    public void ResetData(float holdPoint, float musicValue, float vfxValue, bool isTutorial) {
        this.HoldPoint = holdPoint;
        this.MusicVolume = musicValue;
        this.SoundFXVolume = vfxValue;
        this.isTutorial = isTutorial;
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
public class GameSettingsData : Data {
    public float musicVolume { get; }
    public float soundFXVolume { get; }
    public bool isTutorial { get; }

    public GameSettingsData(float musicVolume, float soundFXVolume, bool isTutorial) {
        this.musicVolume = musicVolume;
        this.soundFXVolume = soundFXVolume;
        this.isTutorial = isTutorial;
    }
}