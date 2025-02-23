using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicDialog : Dialog 
{
    [SerializeField] protected Button closeBtn;

    protected virtual void Start() {
        closeBtn.onClick.AddListener(Close);
    }
}
