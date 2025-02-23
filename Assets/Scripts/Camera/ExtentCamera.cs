using UnityEngine;

public class ExtentCamera : MonoBehaviour
{
    Transform mainCameraTransform;
    float centerOfWorld;
    float mapWidth;
    float offsetFromMainCamera;

    private void Awake() {
        mainCameraTransform = transform.parent;
    }

    private void Start() {
        centerOfWorld = GameSettings.Instance.centerOfWorld;
        mapWidth = GameSettings.Instance.mapWidth;
    }

    private void Update() {
        if (mainCameraTransform.position.x > centerOfWorld)
            offsetFromMainCamera = -mapWidth;
        else offsetFromMainCamera = mapWidth;
        transform.localPosition = new Vector2(offsetFromMainCamera, 0);
    }
}
