using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void Play() {
        AudioManager.Instance.PlayOneShot(audioClip);
    }
}
