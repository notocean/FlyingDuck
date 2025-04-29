using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    private Button button;
    [SerializeField] Canvas talkCanvas;
    [SerializeField] Dialog tutorialDialog;

    GameManager gameManager;

    private void Awake() {
        button = GetComponent<Button>();

        gameManager = GameManager.Instance;
    }

    private void Start() {
        button.onClick.AddListener(OpenTutorial);
    }

    private void OpenTutorial() {
        tutorialDialog.Open();
    }

    void SetVisual(bool doTutorial) {
        talkCanvas.enabled = !doTutorial;
    }

    private void OnEnable() {
        GameManager.Instance.DoTutorialChanged += SetVisual;
    }

    private void OnDisable() {
        if (gameManager != null) {
            gameManager.DoTutorialChanged -= SetVisual;
        }
    }
}
