using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Play, Pause
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null) {
                    _instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private GameState _gameState;
    public GameState gameState {
        get { return _gameState; }
        set {
            _gameState = value;
            if (value == GameState.Pause) {
                Time.timeScale = 0;
            }
            else Time.timeScale = 1;
        }
    }

    private GameObject _player;
    public GameObject Player {
        get {
            if (_player == null) {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            return _player;
        }
        set { _player = value; }
    }

    private LevelManager levelManager;

    bool _doTutorial;
    public bool DoTutorial {
        get {  return _doTutorial; }
        set {
            _doTutorial = value;
            DoTutorialChanged?.Invoke(value);
        }
    }
    public Action<bool> DoTutorialChanged;

    [SerializeField] List<DataManager> dataManagers = new List<DataManager>();
    [SerializeField] GameObject pauseDialogPrefab;

    public int LevelIndex { get; private set; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            levelManager = LevelManager.Instance;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            Application.targetFrameRate = 60;
        }
    }

    public IEnumerator LoadLevel(int index) {
        if (index != 0) {
            LevelData levelData = levelManager.GetCurrentLevelData();
            if (levelData.isSaved) {
                yield return StartCoroutine(LevelDataManager.Instance.LoadLevelData());
            }
            else {
                SaveLevel(true);
            }
        }
    }

    public void SaveLevel(bool includeDefaultLevel = false) {
        if (LevelIndex == 0)
            return;

        LevelData levelData = levelManager.GetCurrentLevelData();

        if (!levelData.isSaved)
            levelData.isSaved = true;

        levelData.objectDataWrapper.Clear();
        LevelDataManager.Instance.SaveLevelData();
        levelManager.SaveLevel();

        if (includeDefaultLevel) {
            levelManager.SetDefaultLevelData(levelData);

            levelManager.SaveDefaultLevel();
        }
    }

    public void ResetLevel() {
        if (LevelIndex == 0)
            return;

        levelManager.ResetLevel();

        ChangeScene(LevelIndex);
    }

    public void ChangeScene(int index) {
        SceneLoader.Instance.LoadScene(index);
    }

    private void SaveData() {
        foreach (DataManager dataManager in dataManagers) {
            dataManager.Save();
        }
        SaveLevel();
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1) {
        LevelIndex = scene.buildIndex;
    }

    private void OnApplicationQuit() {
        SaveData();
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveData();
        }
        else if (LevelIndex != 0) {
            if (gameState != GameState.Pause) {
                DialogManager.Instance.ShowDialog(pauseDialogPrefab.name);
            }
        }
    }
}
