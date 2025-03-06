using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialStep {
    public bool hasAction;
    [SerializeField, ShowIf("hasAction", true)]
    public string instructionText;
    [SerializeField, ShowIf("hasAction")]
    public string actionText;
    
    public bool hasHighlight;
    [SerializeField, ShowIf("hasHighlight")]
    public CanvasGroup canvasGroupHighlight;
    public int talkType;                    
}

public class ShowIfAttribute : PropertyAttribute {
    public string conditionField;
    public bool invertCondition;

    public ShowIfAttribute(string conditionField, bool invertCondition = false) {
        this.conditionField = conditionField;
        this.invertCondition = invertCondition;
    }
}