using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void Play() {
        SoundFXManager.Instance.PlaySoundFX(audioClip);
    }
}
