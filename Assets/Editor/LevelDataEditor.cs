using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(LevelDataManager))]
public class LevelDataEditor : Editor {
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Dictionary<string, Animal> animals = new Dictionary<string, Animal>();
    private GameObject player;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Data")) {
            UpdateData();
        }
    }

    public void UpdateData() {
        player = GameObject.FindGameObjectWithTag("Player");

        SetTileData();
        SetAnimalData();

        List<LevelData> levelDataList = LevelManager.Instance.levelData;
        List<LevelData> defaultLevelDataList = LevelManager.Instance.defaultLevelData;
        int sceneIndex = EditorSceneManager.GetActiveScene().buildIndex;

        // update duck data
        //defaultLevelDataList[sceneIndex - 1].duckData = player.GetComponent<PlayerInfor>().GetDuckData();
        //levelDataList[sceneIndex - 1].duckData = player.GetComponent<PlayerInfor>().GetDuckData();

        // update tiles data
        defaultLevelDataList[sceneIndex - 1].tileData.Clear();
        levelDataList[sceneIndex - 1].tileData.Clear();
        foreach (KeyValuePair<string, Tile> tile in tiles) {
            defaultLevelDataList[sceneIndex - 1].tileData.Add(tile.Key, new TileDataWrapper(tile.Value.GetTileData()));
            levelDataList[sceneIndex - 1].tileData.Add(tile.Key, new TileDataWrapper(tile.Value.GetTileData()));
        }

        // update animals data
        defaultLevelDataList[sceneIndex - 1].animalData.Clear();
        levelDataList[sceneIndex - 1].animalData.Clear();
        foreach (KeyValuePair<string, Animal> animal in animals) {
            defaultLevelDataList[sceneIndex - 1].animalData.Add(animal.Key, new AnimalDataWrapper(animal.Value.GetAnimalData()));
            levelDataList[sceneIndex - 1].animalData.Add(animal.Key, new AnimalDataWrapper(animal.Value.GetAnimalData()));
        }
    }

    private void SetTileData() {
        // get object from top to bottom in hierarchy
        GameObject[] list = GameObject.FindGameObjectsWithTag("Tile");

        List<GameObject> t = list
            .OrderBy(obj => obj.transform.parent?.GetSiblingIndex())
            .ThenBy(obj => obj.transform.GetSiblingIndex())
            .ToList();

        // set tiles data 
        tiles.Clear();
        for (int i = 0; i < t.Count; i++) {
            if (t[i] != null) {
                t[i].name = $"Tile {i + 1}";

                Tile tile = t[i].GetComponent<Tile>();
                if (tile != null) {
                    tiles.Add(t[i].name, tile);
                }
            }
        }
    }

    private void SetAnimalData() {
        // get object from top to bottom in hierarchy
        GameObject[] list = GameObject.FindGameObjectsWithTag("Animal");

        List<GameObject> a = list
            .OrderBy(obj => obj.transform.parent?.parent.GetSiblingIndex())
            .ThenBy(obj => obj.transform.parent?.GetSiblingIndex())
            .ToList();

        // set animals data 
        animals.Clear();
        for (int i = 0; i < a.Count; i++) {
            if (a[i] != null) {
                a[i].name = $"Animal {i + 1}";

                Animal animal = a[i].GetComponent<Animal>();
                if (animal != null) {
                    animals.Add(a[i].name, animal);
                }
            }
        }
    }
}