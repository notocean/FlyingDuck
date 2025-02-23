using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (levelManager.currentLevelIndex <= levelManager.maxAvaiableLevelIndex) {
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
