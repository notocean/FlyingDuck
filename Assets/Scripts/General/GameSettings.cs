using System;
using UnityEngine;
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

    [Range(0f, 1f)]
    public float holdPoint;

    [HideInInspector] public UnityEvent onMusicValueChanged = new UnityEvent();

    [SerializeField]
    [Range(0f, 1f)]
    private float musicValue;
    public float MusicValue {
        get { return musicValue; }
        set {
            musicValue = Mathf.Clamp(value, 0, 1);
            onMusicValueChanged.Invoke();
        }
    }

    [HideInInspector] public UnityEvent onVfxValueChanged = new UnityEvent();

    [SerializeField]
    [Range(0f, 1f)]
    private float vfxValue;

    public float VfxValue {
        get { return vfxValue; }
        set {
            vfxValue = Mathf.Clamp(value, 0, 1);
            onVfxValueChanged.Invoke();
        }
    }

    [SerializeField]
    private bool isTutorial;

    public bool IsTutorial {
        get { return isTutorial; }
        set {
            isTutorial = value;
        }
    }

    const string filename = "GameSettings";

    [ExecuteAlways]
    public override void Save() {
        SaveLoadManager.Save(new GameSettingsData(musicValue, vfxValue, isTutorial), filename);
    }

    public override void Load() {
        Data data = SaveLoadManager.Load(filename);

        if (data != null) {
            if (data is GameSettingsData gameSettingsData) {
                musicValue = gameSettingsData.musicValue;
                vfxValue = gameSettingsData.vfxValue;
                isTutorial = gameSettingsData.isTutorial;
            }
        }
    }

    private void OnEnable() {
        Load();
    }
}

[Serializable]
public class GameSettingsData : Data {
    public float musicValue { get; }
    public float vfxValue { get; }
    public bool isTutorial { get; }

    public GameSettingsData(float musicValue, float vfxValue, bool isTutorial) {
        this.musicValue = musicValue;
        this.vfxValue = vfxValue;
        this.isTutorial = isTutorial;
    }
}