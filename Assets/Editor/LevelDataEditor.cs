using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelDataManager))]
public class LevelDataEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Tile Name")) {
            UpdateTileName();
        }

        if (GUILayout.Button("Update Animal & Plant Name")) {
            UpdateAnimalAndPlantName();
        }
    }

    private void UpdateTileName() {
        // get object from top to bottom in hierarchy
        GameObject[] list = GameObject.FindGameObjectsWithTag("Tile");

        List<GameObject> t = list
            .OrderBy(obj => obj.transform.parent?.GetSiblingIndex())
            .ThenBy(obj => obj.transform.GetSiblingIndex())
            .ToList();

        for (int i = 0; i < t.Count; i++) {
            if (t[i] != null) {
                t[i].name = $"Tile {i + 1}";
            }
        }
    }

    private void UpdateAnimalAndPlantName() {
        // get object from top to bottom in hierarchy
        GameObject[] animalList = GameObject.FindGameObjectsWithTag("Animal");

        HashSet<string> animalNameList = new();
        foreach (GameObject obj in animalList) {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (prefab != null) {
                if (!animalNameList.Contains(prefab.name)) {
                    animalNameList.Add(prefab.name);
                }
            }
        }

        foreach (string name in animalNameList) {
            int index = 1;
            foreach (GameObject obj in animalList) {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                if (prefab != null) {
                    if (prefab.name.Equals(name)) {
                        obj.name = name + $" {index}";
                        index++;
                    }
                }
            }
        }

        GameObject[] plantList = GameObject.FindGameObjectsWithTag("Plant");

        HashSet<string> plantNameList = new();
        foreach (GameObject obj in plantList) {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (prefab != null) {
                if (!plantNameList.Contains(prefab.name)) {
                    plantNameList.Add(prefab.name);
                }
            }
        }

        foreach (string name in plantNameList) {
            int index = 1;
            foreach (GameObject obj in plantList) {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                if (prefab != null) {
                    if (prefab.name.Equals(name)) {
                        obj.name = name + $" {index}";
                        index++;
                    }
                }
            }
        }
    }
}