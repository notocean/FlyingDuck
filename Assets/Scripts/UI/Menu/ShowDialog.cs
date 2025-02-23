using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShowDialog : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(Show);
    }

    private void Show() {
        dialog.Open();
    }
}
