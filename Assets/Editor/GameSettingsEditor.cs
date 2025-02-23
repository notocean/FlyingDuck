using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GameSettings settings = (GameSettings)target;
        if (GUILayout.Button("Save")) {
            settings.Save();
        }
    }
}
