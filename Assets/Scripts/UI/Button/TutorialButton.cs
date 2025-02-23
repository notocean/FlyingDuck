using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    private Button button;
    [SerializeField] Tutorial tutorial;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OpenTutorial);
    }

    private void OpenTutorial() {
        tutorial.Open();
    }
}
