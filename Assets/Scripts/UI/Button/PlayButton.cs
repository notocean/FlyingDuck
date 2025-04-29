using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class PlayButton : MonoBehaviour
{
    private Button button;
    private Image image;
    private LevelManager levelManager;

    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        levelManager = LevelManager.Instance;
    }

    private void Start() {
        button.onClick.AddListener(Play);
        SetVisual();
    }

    private void Play() {
        int currentIndexLevel = levelManager.currentLevelIndex;
        int maxIndexLevel = levelManager.maxActiveLevelIndex;
        if (currentIndexLevel <= maxIndexLevel)
            GameManager.Instance.ChangeScene(currentIndexLevel + 1);
    }

    private void SetVisual() {
        if (levelManager.IsActiveLevel()) {
            image.color = Color.white;
            button.interactable = true;
        }
        else {
            image.color = new Color(0.6f, 0.6f, 0.6f);
            button.interactable = false;
        }
    }

    private void OnEnable() {
        levelManager.levelSelectedChanged.AddListener(SetVisual);
    }

    private void OnDisable() {
        levelManager.levelSelectedChanged.RemoveListener(SetVisual);
    }
}
