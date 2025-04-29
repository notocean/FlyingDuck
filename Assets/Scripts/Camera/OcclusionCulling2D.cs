using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OcclusionCulling2D : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraTop;
    private float cameraBottom;

    [SerializeField] private List<OcclusionCulling2DObject> occlusionCulling2DObjects = new List<OcclusionCulling2DObject>();

    private void Awake() {
        mainCamera = GetComponent<Camera>();

        occlusionCulling2DObjects = FindObjectsByType<OcclusionCulling2DObject>(FindObjectsSortMode.None).ToList<OcclusionCulling2DObject>();
    }

    private void Update() {
        cameraTop = transform.position.y + mainCamera.orthographicSize;
        cameraBottom = transform.position.y - mainCamera.orthographicSize;

        foreach (OcclusionCulling2DObject obj in occlusionCulling2DObjects) {
            bool isVisible = obj.top > cameraBottom & obj.bottom < cameraTop;

            obj.SetVisible(isVisible);
        }
    }
}
