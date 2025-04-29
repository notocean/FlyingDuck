using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ChangeLevelButton : MonoBehaviour
{
    enum ChangeLevelType {
        Left, Right
    }

    [SerializeField] private ChangeLevelType type;
    private Button button;
    private Image image;
    private LevelManager levelManager;

    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        levelManager = LevelManager.Instance;
    }

    private void Start() {
        button.onClick.AddListener(ChangeLevel);
        SetVisual();
    }

    private void ChangeLevel() {
        switch (type) {
            case ChangeLevelType.Left:
                levelManager.ChangeCurrentLevel(-1);
                break;
            case ChangeLevelType.Right:
                levelManager.ChangeCurrentLevel(1);
                break;
            default:
                break;
        }
    }

    private void SetVisual() {
        switch (type) {
            case ChangeLevelType.Left:
                if (levelManager.currentLevelIndex > 0) {
                    image.enabled = true;
                }
                else image.enabled = false;
                break;
            case ChangeLevelType.Right:
                if (levelManager.currentLevelIndex < levelManager.levelUISprites.Count - 1) {
                    image.enabled = true;
                }
                else image.enabled = false;
                break;
            default:
                break;
        }
    }

    private void OnEnable() {
        levelManager.levelSelectedChanged.AddListener(SetVisual);
    }

    private void OnDisable() {
        levelManager.levelSelectedChanged.RemoveListener(SetVisual);
    }
}
