using UnityEngine;

public class AvaiableMessage : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float alpha;
    private LevelManager levelManager;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        levelManager = LevelManager.Instance;
    }

    private void Start() {
        DisplayMessage();
    }

    private void DisplayMessage() {
        if (levelManager.currentLevelIndex <= levelManager.MaxAvaiableLevelIndex) {
            canvasGroup.alpha = 0f;
        }
        else canvasGroup.alpha = alpha;
    }

    private void OnEnable() {
        levelManager.levelSelectedChanged.AddListener(DisplayMessage);
    }

    private void OnDisable() {
        levelManager.levelSelectedChanged.RemoveListener(DisplayMessage);
    }
}
