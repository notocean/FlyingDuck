using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitItemManager : MonoBehaviour
{
    [SerializeField] private GameObject outfitUIObj;
    private List<OutfitItemUI> outfitItems = new List<OutfitItemUI>();
    private OutfitItemUI currentSelectedItem;
    bool wasGenerated = false;

    IEnumerator Display() {
        yield return null;

        if (!wasGenerated) {
            Generate();
        }

        Refresh();
    }

    private void Generate() {
        wasGenerated = true;

        List<HairOutfit> hairOutfits = HairOutfitManager.Instance.hairOutfits;
        for (int i = 0; i < hairOutfits.Count; i++) {
            outfitItems.Add(Instantiate(outfitUIObj, transform, false).GetComponentInChildren<OutfitItemUI>());
            outfitItems[i].Initial(hairOutfits[i]);
            if (i == HairOutfitManager.Instance.currentHairIndex) {
                currentSelectedItem = outfitItems[i];
                currentSelectedItem.SetVisual(true);
            }
        }
    }

    private void Refresh() {
        foreach (OutfitItemUI outfitItemUI in outfitItems) {
            HairOutfit hairOutfit = outfitItemUI.hairOutfit;
            outfitItemUI.SetActive(hairOutfit.isActive);
            outfitItemUI.SetAttention(hairOutfit.hasAttention);
        }
    }

    private void SetSelectedItem() {
        currentSelectedItem.SetVisual(false);
        currentSelectedItem = outfitItems[HairOutfitManager.Instance.currentHairIndex];
        currentSelectedItem.SetVisual(true);
    }

    private void OnEnable() {
        HairOutfitManager.Instance.hairOutfitChanged += SetSelectedItem;

        StartCoroutine(Display());
    }

    private void OnDisable() {
        HairOutfitManager.Instance.hairOutfitChanged -= SetSelectedItem;
    }
}
