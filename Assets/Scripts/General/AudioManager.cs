using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null) {
                    _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
                }
            }

            return _instance;
        }
    }

    private AudioSource musicSource;
    private AudioSource vfxSource;

    private void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        vfxSource = gameObject.AddComponent<AudioSource>();
        SetMusicValue();
        SetVfxValue();
    }

    public void PlayOneShot(AudioClip audioClip) {
        vfxSource.PlayOneShot(audioClip);
    }

    public AudioSource GetMusicAudioSource() {
        return musicSource;
    }

    public AudioSource GetVfxAudioSource() {
        return vfxSource;
    }

    private void SetMusicValue() {
        musicSource.volume = GameSettings.Instance.MusicValue;
    }

    private void SetVfxValue() {
        vfxSource.volume = GameSettings.Instance.VfxValue;
    }

    private void OnEnable() {
        GameSettings.Instance.onMusicValueChanged.AddListener(SetMusicValue);
        GameSettings.Instance.onVfxValueChanged.AddListener(SetVfxValue);
    }

    private void OnDisable() {
        GameSettings.Instance.onMusicValueChanged.RemoveListener(SetMusicValue);
        GameSettings.Instance.onVfxValueChanged.RemoveListener(SetVfxValue);
    }
}
