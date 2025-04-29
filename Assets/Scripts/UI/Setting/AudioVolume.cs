using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioVolume : MonoBehaviour
{
    enum AudioType {
        Music, Vfx
    }

    [SerializeField] private AudioType type;
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    private void Start() {
        SetVisual();
        slider.onValueChanged.AddListener(ValueChangedHandle);
    }

    private void SetVisual() {
        switch (type) {
            case AudioType.Music:
                slider.value = GameSettings.Instance.MusicVolume;
                break;
            case AudioType.Vfx:
                slider.value = GameSettings.Instance.SoundFXVolume;
                break;
            default:
                break;
        }
    }

    private void ValueChangedHandle(float value) {
        switch (type) {
            case AudioType.Music:
                GameSettings.Instance.MusicVolume = value;
                MusicManager.Instance.SetMusicVolume(value);
                break;
            case AudioType.Vfx:
                GameSettings.Instance.SoundFXVolume = value;
                SoundFXManager.Instance.SetSoundFXVolume(value);
                break;
            default:
                break;
        }
    }
}
