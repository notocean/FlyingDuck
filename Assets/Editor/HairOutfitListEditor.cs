using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HairOutfitList))]
public class HairOutfitListEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        HairOutfitList hairOutfitLists = (HairOutfitList)target;
        if (GUILayout.Button("Save")) {
            hairOutfitLists.Save();
        }
    }
}
