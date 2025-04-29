using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResetData))]
public class ResetDataEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        ResetData resetData = (ResetData)target;
        if (GUILayout.Button("Reset")) {
            resetData.Reset();
        }
    }
}
