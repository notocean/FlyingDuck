using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PharmaceuticalItemManager : MonoBehaviour {
    List<Pharmaceutical> pharmaceuticals;
    [SerializeField] private GameObject pharmaceuticalUIObj;
    private List<PharmaceuticalItemUI> pharmaceuticalItems = new List<PharmaceuticalItemUI>();
    private PharmaceuticalItemUI currentSelectedItem;

    private void Awake() {
        pharmaceuticals = PharmaceuticalList.Instance.pharmaceuticals;
    }

    private void Start() {
        int index = PharmaceuticalList.Instance.currentPharmaceuticalIndex;
        for (int i = 0; i < pharmaceuticals.Count; i++) {
            pharmaceuticalItems.Add(Instantiate(pharmaceuticalUIObj, transform, false).GetComponentInChildren<PharmaceuticalItemUI>());
            pharmaceuticalItems[i].Initial(pharmaceuticals[i], this, i);
            if (i == index) {
                currentSelectedItem = pharmaceuticalItems[i];
                currentSelectedItem.SetVisual(true);
            }
        }
    }

    private void SetSelectedItem(int index) {
        currentSelectedItem.SetVisual(false);
        currentSelectedItem = pharmaceuticalItems[index];
        currentSelectedItem.SetVisual(true);
    }

    private void ActivePharmaceutical(int index) {
        pharmaceuticalItems[index].SetActive(true);
    }

    private void OnEnable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SetSelectedItem);
        PharmaceuticalList.Instance.isActiveEvent.AddListener(ActivePharmaceutical);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.pharmaceuticalChanged.AddListener(SetSelectedItem);
        PharmaceuticalList.Instance.isActiveEvent.AddListener(ActivePharmaceutical);
    }
}
