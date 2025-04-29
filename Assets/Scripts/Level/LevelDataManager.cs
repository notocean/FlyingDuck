using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    private static LevelDataManager _instance;
    public static LevelDataManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<LevelDataManager>();
                if (_instance == null) {
                    _instance = new GameObject("LevelDataManager").AddComponent<LevelDataManager>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<string, ISaveableObject> saveableObjects = new Dictionary<string, ISaveableObject>();
    LevelData levelData;

    // Đảm bảo rằng các đối tượng đã đăng ký vào saveableObjects
    bool isReady = false;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
            levelData = LevelManager.Instance.GetCurrentLevelData();
            StartCoroutine(WaitForEndFrame());
        }
    }

    IEnumerator WaitForEndFrame() {
        // Chờ đến frame tiếp theo
        yield return null;
        isReady = true;
    }

    public void RegisterSaveableObject(string name, ISaveableObject saveableObject) {
        saveableObjects.Add(name, saveableObject);
    }

    public IEnumerator LoadLevelData() {
        yield return new WaitUntil(() => isReady);

        foreach (KeyValuePair<string, ISaveableObject> saveableObject in saveableObjects) {
            saveableObject.Value.SetObjectData(levelData.objectDataWrapper[saveableObject.Key].data);
        }
    }

    public IEnumerator LoadLevelData(string name) {
        yield return new WaitUntil(() => isReady);

        saveableObjects[name].SetObjectData(levelData.objectDataWrapper[name].data);
    }

    public void SaveLevelData() {
        foreach (KeyValuePair<string, ISaveableObject> saveableObject in saveableObjects) {
            levelData.objectDataWrapper.Add(saveableObject.Key, new ObjectDataWrapper(saveableObject.Value.GetObjectData()));
        }
    }
}
