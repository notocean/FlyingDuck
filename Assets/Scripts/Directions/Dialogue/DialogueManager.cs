using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(Collider2D))]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] CanvasGroup dialogueCanvasGroup;
    [SerializeField] TMP_Text dialogueText;

    public IEnumerator DisplayDialogue(string text, float delayWordTime) {
        dialogueCanvasGroup.alpha = 1f;

        string[] words = text.Split(' ');

        dialogueText.text = "";

        foreach (string word in words) {
            dialogueText.text += word + " ";
            yield return new WaitForSeconds(delayWordTime);
        }
    }

    public void HideDialogue() {
        dialogueCanvasGroup.alpha = 0f;
    }
}
