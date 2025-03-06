using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(Exit);
    }

    void Exit() {

    }
}
