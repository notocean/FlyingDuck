using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    Transform playerTransform;
    Vector3 pos;
    [SerializeField] Vector2 offset;

    private void Start() {
        playerTransform = GameManager.Instance.Player.transform;
    }

    private void Update() {
        pos.x = playerTransform.position.x + offset.x;
        pos.y = playerTransform.position.y + offset.y;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
