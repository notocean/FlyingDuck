using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimal : MonoBehaviour
{
    [SerializeField] protected List<string> dialogues;
    [SerializeField] protected float delayWordTime;
    [SerializeField] protected float stepTime;
    protected int dialogueIndex = 0;
    protected bool canDialogue = true;

    protected DialogueManager dialogueManager;

    protected virtual void Awake() {
        dialogueManager = GetComponent<DialogueManager>();
    }

    protected IEnumerator CoolDown() {
        yield return new WaitForSeconds(stepTime * 2 / 3);
        dialogueManager.HideDialogue();

        yield return new WaitForSeconds(stepTime);
        canDialogue = true;
    }
}
