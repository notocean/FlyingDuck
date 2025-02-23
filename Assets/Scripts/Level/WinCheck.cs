using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WinCheck : MonoBehaviour
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private AudioPlayer audioPlayer;
    private bool isWin = false;

    private void Awake() {
        dialog.showEvent.AddListener(MusicHandle);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isWin) {
            if ((LayerMask.GetMask("Player") & (1 << collision.gameObject.layer)) != 0) {
                isWin = true;
                StartCoroutine(OpenWinningDialog());
            }
        }
    }

    IEnumerator OpenWinningDialog() {
        yield return new WaitForSeconds(0.5f);
        dialog.Open();
        audioPlayer.Play();
    }

    private void MusicHandle(bool showDialog) {
        if (showDialog) 
            AudioManager.Instance.GetMusicAudioSource().Stop();
        else AudioManager.Instance.GetMusicAudioSource().Play();
    }
}
