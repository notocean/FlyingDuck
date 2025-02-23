using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PlayerData playerData = (PlayerData)target;
        if (GUILayout.Button("Save")) {
            playerData.Save();
        }
    }
}
