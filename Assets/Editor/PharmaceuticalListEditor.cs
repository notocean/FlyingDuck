using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PharmaceuticalList))]
public class PharmaceuticalListEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PharmaceuticalList pharmaceuticalList = (PharmaceuticalList)target;
        if (GUILayout.Button("Save")) {
            pharmaceuticalList.Save();
        }
    }
}
