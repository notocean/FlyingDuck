using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        LevelManager levelManager = (LevelManager)target;
        if (GUILayout.Button("Save")) {
            levelManager.SaveGeneral();
            for (int i = 0; i < levelManager.levelData.Count; i++) {
                levelManager.SaveLevel(i);
            }
        }
    }
}
