using System.Collections;
using System.Collections.Generic;
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
        image.sprite = HairOutfitList.Instance.GetCurrentHair();
    }

    private void OnEnable() {
        HairOutfitList.Instance.hairOutfitChanged.AddListener(SetHair);
    }

    private void OnDisable() {
        HairOutfitList.Instance.hairOutfitChanged.RemoveListener(SetHair);
    }
}
