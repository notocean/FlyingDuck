using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidableButton : MonoBehaviour
{
    protected Button button;
    protected Image image;

    protected virtual void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    protected virtual void Start() {
        button.onClick.AddListener(ClickedHandle);
    }

    public void SetVisual(bool isActive) {
        if (isActive) {
            image.color = Color.white;
            button.interactable = true;
        }
        else {
            image.color = new Color(0.6f, 0.6f, 0.6f);
            button.interactable = false;
        }
    }

    protected virtual void ClickedHandle() { }
}
