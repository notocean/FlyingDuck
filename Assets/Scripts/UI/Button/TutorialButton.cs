using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    private Button button;
    [SerializeField] Canvas talkCanvas;
    [SerializeField] Dialog tutorialDialog;

    private void Awake() {
        button = GetComponent<Button>();
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
        GameManager.Instance.DoTutorialChanged -= SetVisual;
    }
}
