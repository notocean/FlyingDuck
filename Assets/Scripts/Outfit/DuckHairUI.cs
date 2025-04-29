using UnityEngine;
using UnityEngine.UI;

public class DuckHairUI : MonoBehaviour 
{
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start() {
        SetHair();
    }

    private void SetHair() {
        image.sprite = HairOutfitManager.Instance.GetCurrentHair();
    }

    private void OnEnable() {
        HairOutfitManager.Instance.hairOutfitChanged += SetHair;
    }

    private void OnDisable() {
        HairOutfitManager.Instance.hairOutfitChanged -= SetHair;
    }
}
