using UnityEditor;
using UnityEngine;

public class OffsetSelectedObjects : EditorWindow {
    Vector3 offset = Vector3.zero;

    [MenuItem("Tools/Offset Selected Objects")]
    static void Init() {
        OffsetSelectedObjects window = (OffsetSelectedObjects)EditorWindow.GetWindow(typeof(OffsetSelectedObjects));
        window.Show();
    }

    void OnGUI() {
        GUILayout.Label("Offset selected objects", EditorStyles.boldLabel);
        offset = EditorGUILayout.Vector3Field("Offset", offset);

        if (GUILayout.Button("Move objects")) {
            foreach (GameObject obj in Selection.gameObjects) {
                Undo.RecordObject(obj.transform, "Offset Position");
                obj.transform.position += offset;
            }
        }
    }
}
