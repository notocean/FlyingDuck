
using UnityEngine;

public class DuckHair : MonoBehaviour 
{
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        SetHair();
    }

    private void SetHair() {
        spriteRenderer.sprite = HairOutfitManager.Instance.GetCurrentHair();
    }

    private void OnEnable() {
        HairOutfitManager.Instance.hairOutfitChanged += SetHair;
    }

    private void OnDisable() {
        HairOutfitManager.Instance.hairOutfitChanged -= SetHair;
    }
}
