using UnityEngine;

public class Dove : DialogueAnimal
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (canDialogue) {
            canDialogue = false;

            StartCoroutine(dialogueManager.DisplayDialogue(dialogues[dialogueIndex], delayWordTime));
            dialogueIndex = (dialogueIndex + 1) % dialogues.Count;
            StartCoroutine(CoolDown());
        }
    }
}
