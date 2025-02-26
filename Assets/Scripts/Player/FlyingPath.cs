using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlyingPath : MonoBehaviour {
    [SerializeField] List<Vector3> points = new List<Vector3>();

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        for (int i = 0; i < points.Count - 1; i++) {
            Gizmos.DrawLine(transform.position + points[i], transform.position + points[i + 1]);
        }
    }
}