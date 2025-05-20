using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextLevel : MonoBehaviour
{
    private Button button;
    private LevelManager levelManager;
    [SerializeField] Dialog currentDialog;

    private void Awake() {
        button = GetComponent<Button>();
        levelManager = LevelManager.Instance;
    }

    private void Start() {
        button.onClick.AddListener(Next);
    }

    private void Next() {
        levelManager.ResetLevel();
        currentDialog.Close();
        if (levelManager.currentLevelIndex < levelManager.maxActiveLevelIndex) {
            levelManager.ChangeCurrentLevel(1);
            GameManager.Instance.ChangeScene(levelManager.currentLevelIndex + 1);
        }
        else GameManager.Instance.ChangeScene(0);
    }
}
