using System.Collections;
using System.Collections.Generic;
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
        levelManager.ChangeCurrentLevel(1);
        int currentIndexLevel = levelManager.currentLevelIndex;
        int maxIndexLevel = levelManager.maxActiveLevelIndex;
        if (currentIndexLevel <= maxIndexLevel)
            GameManager.Instance.ChangeScene(currentIndexLevel + 1);
        else GameManager.Instance.ChangeScene(0);
    }
}
