using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WinCheck : MonoBehaviour
{
    [SerializeField] GameObject dialogPrefab;
    [SerializeField] private AudioClip winningAudioClip;
    private bool isWin = false;

    private void Start() {
        DialogManager.Instance.RegisterDialog(dialogPrefab.name, dialogPrefab);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isWin) {
            isWin = true;
            StartCoroutine(OpenWinningDialog());
        }
    }

    IEnumerator OpenWinningDialog() {
        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.IncreaseMaxActiveLevel();
        DialogManager.Instance.ShowDialog(dialogPrefab.name);
        SoundFXManager.Instance.PlaySoundFX(winningAudioClip);
    }
}
