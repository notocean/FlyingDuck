using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitItemManager : MonoBehaviour
{
    [SerializeField] private GameObject outfitUIObj;
    private List<OutfitItemUI> outfitItems = new List<OutfitItemUI>();
    private OutfitItemUI currentSelectedItem;

    private void Start() {
        List<HairOutfit> hairOutfits = HairOutfitList.Instance.hairOutfits;
        for (int i = 0; i < hairOutfits.Count; i++) {
            outfitItems.Add(Instantiate(outfitUIObj, transform, false).GetComponentInChildren<OutfitItemUI>());
            outfitItems[i].Initial(hairOutfits[i]);
            if (i == HairOutfitList.Instance.currentHairIndex) {
                currentSelectedItem = outfitItems[i];
                currentSelectedItem.SetVisual(true);
            }
        }
    }

    private void ActiveOutfit(int index) {
        outfitItems[index].SetActive(true);
    }

    private void SetSelectedItem() {
        currentSelectedItem.SetVisual(false);
        currentSelectedItem = outfitItems[HairOutfitList.Instance.currentHairIndex];
        currentSelectedItem.SetVisual(true);
    }

    private void OnEnable() {
        HairOutfitList.Instance.hairOutfitChanged.AddListener(SetSelectedItem);
        HairOutfitList.Instance.hairOutfitActived.AddListener(ActiveOutfit);
    }

    private void OnDisable() {
        HairOutfitList.Instance.hairOutfitChanged.RemoveListener(SetSelectedItem);
        HairOutfitList.Instance.hairOutfitActived.RemoveListener(ActiveOutfit);
    }
}
