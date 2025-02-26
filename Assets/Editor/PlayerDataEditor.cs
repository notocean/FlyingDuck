using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDataManager))]
public class PlayerDataEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PlayerDataManager playerData = (PlayerDataManager)target;
        if (GUILayout.Button("Save")) {
            playerData.Save();
        }
    }
}
