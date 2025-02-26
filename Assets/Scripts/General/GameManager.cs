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
    private LevelDataManager levelDataManager;

    [SerializeField] List<DataManager> dataManagers = new List<DataManager>();

    private void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        levelManager = LevelManager.Instance;
    }

    public IEnumerator LoadLevel(int index) {
        if (index != 0) {
            LevelData levelData = levelManager.GetCurrentLevelData();

            if (levelData.isSave) {
                yield return new WaitUntil(() => levelDataManager != null);

                foreach (KeyValuePair<string, Tile> tile in levelDataManager.GetTileObjects()) {
                    tile.Value.SetTileData(levelData.tileData[tile.Key].data);
                }

                foreach (KeyValuePair<string, Animal> animal in levelDataManager.GetAnimalObjects()) {
                    animal.Value.SetAnimalData(levelData.animalData[animal.Key].data);
                }
            }

            Player.GetComponent<PlayerController>().SetPlayerData(levelData.playerData);
        }
    }

    public void SaveLevel() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        LevelData levelData = levelManager.GetCurrentLevelData();
        levelData.isSave = true;
        levelData.playerData = Player.GetComponent<PlayerController>().GetPlayerData();

        if (levelDataManager != null) {
            levelData.tileData.Clear();
            foreach (KeyValuePair<string, Tile> tile in levelDataManager.GetTileObjects()) {
                levelData.tileData.Add(tile.Key, new TileDataWrapper(tile.Value.GetTileData()));
            }

            levelData.animalData.Clear();
            foreach (KeyValuePair<string, Animal> animal in levelDataManager.GetAnimalObjects()) {
                levelData.animalData.Add(animal.Key, new AnimalDataWrapper(animal.Value.GetAnimalData()));
            }
        }
        levelManager.SaveLevel(levelManager.currentLevelIndex);
    }

    public void ResetLevel() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        levelManager.ResetLevel();

        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeScene(int index) {
        SceneLoader.Instance.LoadScene(index);
    }

    public void SetLevelDataManager(LevelDataManager levelDataManager) {
        this.levelDataManager = levelDataManager;
    }

    private void SaveData() {
        foreach (DataManager dataManager in dataManagers) {
            dataManager.Save();
        }
        SaveLevel();
    }

    private void OnApplicationQuit() {
        SaveData();
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveData();
        }
    }
}
