using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private AudioSource soundFXObject;
    private Queue<AudioSource> soundFXQueue = new Queue<AudioSource>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() {
        SetSoundFXVolume(GameSettings.Instance.SoundFXVolume);
    }

    public void SetSoundFXVolume(float volume) {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void PlaySoundFX(AudioClip clip) {
        AudioSource audioSource = GetAudioSource();
        audioSource.clip = clip;
        audioSource.Play();

        float clipLength = clip.length;
        StartCoroutine(CountDownToEnQueue(audioSource, clipLength));
    }

    IEnumerator CountDownToEnQueue(AudioSource audioSource, float time) {
        yield return new WaitForSeconds(time);
        ReturnAudioSource(audioSource);
    }

    public AudioSource GetAudioSource() {
        if (soundFXQueue.Count <= 0) {
            soundFXQueue.Enqueue(Instantiate(soundFXObject, transform));
        }

        return soundFXQueue.Dequeue();
    }

    public void ReturnAudioSource(AudioSource audioSource) {
        soundFXQueue.Enqueue(audioSource);
    }
}
