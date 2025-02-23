using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling2DObject : MonoBehaviour
{
    [SerializeField] Vector2 size = Vector2.one;
    [SerializeField] Vector2 offset = Vector2.zero;

    private Vector2 center { get { return (Vector2) transform.position + offset; } }
    
    public float top { get { return center.y + size.y; } }
    public float bottom { get { return center.y - size.y; } }

    bool isVisible = true;

    public void SetVisible(bool isVisible) {
        if (this.isVisible != isVisible) {
            this.isVisible = isVisible;

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
                renderer.enabled = isVisible;
            }

            foreach (Collider2D collider2D in GetComponentsInChildren<Collider2D>()) {
                collider2D.enabled = isVisible;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(-5, top), new Vector2(5, top));
        Gizmos.DrawLine(new Vector2(-5, bottom), new Vector2(5, bottom));
    }
}
