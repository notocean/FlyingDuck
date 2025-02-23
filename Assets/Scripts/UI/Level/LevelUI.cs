using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LevelUI : MonoBehaviour
{
    enum LevelUIType {
        Top, Left, Center, Right
    }

    [SerializeField] private LevelUIType type;
    private Image image;
    private LevelManager levelManager;

    private void Awake() {
        image = GetComponent<Image>();
        levelManager = LevelManager.Instance;
    }

    private void Start() {
        SetVisual();
    }

    private void SetVisual() {
        LevelSprite levelSprite = levelManager.GetCurrentLevelSprite();
        Sprite sprite = null;
        switch (type) {
            case LevelUIType.Top:
                sprite = levelSprite.topSprite;
                break;
            case LevelUIType.Left:
                sprite = levelSprite.leftSprite;
                break;
            case LevelUIType.Center:
                sprite = levelSprite.centerSprite;
                break;
            case LevelUIType.Right:
                sprite = levelSprite.rightSprite;
                break;
            default:
                break;
        }

        image.sprite = sprite;
        image.SetNativeSize();
        if (levelManager.IsActiveLevel()) {
            image.color = Color.white;
        }
        else {
            image.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnEnable() {
        levelManager.levelSelectedChanged.AddListener(SetVisual);
    }

    private void OnDisable() {
        levelManager.levelSelectedChanged.RemoveListener(SetVisual);
    }
}
