using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioController : MonoBehaviour
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
                slider.value = GameSettings.Instance.MusicValue;
                break;
            case AudioType.Vfx:
                slider.value = GameSettings.Instance.VfxValue;
                break;
            default:
                break;
        }
    }

    private void ValueChangedHandle(float value) {
        switch (type) {
            case AudioType.Music:
                GameSettings.Instance.MusicValue = value;
                break;
            case AudioType.Vfx:
                GameSettings.Instance.VfxValue = value;
                break;
            default:
                break;
        }
    }
}
