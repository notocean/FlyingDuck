using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RepeatMovingTile))]
public class RepeatMoveingTileEditor : Editor {
    int pointIndex = 0;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        RepeatMovingTile repeatMovingTile = (RepeatMovingTile)target;

        if (GUILayout.Button("Add point")) {
            repeatMovingTile.AddPoint();
        }

        pointIndex = EditorGUILayout.IntField("Point index: ", pointIndex);

        if (GUILayout.Button("Update position to point")) {
            repeatMovingTile.UpdatePosition(pointIndex);
        }
    }
}
