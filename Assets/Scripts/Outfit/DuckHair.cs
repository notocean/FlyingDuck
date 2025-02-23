using System.Collections;
using System.Collections.Generic;
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
        spriteRenderer.sprite = HairOutfitList.Instance.GetCurrentHair();
    }

    private void OnEnable() {
        HairOutfitList.Instance.hairOutfitChanged.AddListener(SetHair);
    }

    private void OnDisable() {
        HairOutfitList.Instance.hairOutfitChanged.RemoveListener(SetHair);
    }
}
