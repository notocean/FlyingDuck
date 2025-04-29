using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmaceuticalItemManager : MonoBehaviour {
    List<Pharmaceutical> pharmaceuticals;
    [SerializeField] private GameObject pharmaceuticalUIObj;
    private List<PharmaceuticalItemUI> pharmaceuticalItems = new List<PharmaceuticalItemUI>();
    private PharmaceuticalItemUI currentSelectedItem;
    bool wasGenerated = false;

    private void Awake() {
        pharmaceuticals = PharmaceuticalManager.Instance.pharmaceuticalList;
    }

    IEnumerator Display() {
        yield return null;

        if (!wasGenerated) {
            Generate();
        }

        Refresh();
    }

    private void Generate() {
        wasGenerated = true;

        // Tạo các hình ảnh dược phẩm
        int index = PharmaceuticalManager.Instance.currentPharmaceuticalIndex;
        for (int i = 0; i < pharmaceuticals.Count; i++) {
            pharmaceuticalItems.Add(Instantiate(pharmaceuticalUIObj, transform, false).GetComponentInChildren<PharmaceuticalItemUI>());
            pharmaceuticalItems[i].Initial(pharmaceuticals[i]);
            if (i == index) {
                SetSelectedItem(i);
            }
        }
    }

    private void Refresh() {
        // Cập nhật các dược phẩm
        int currentSelectedIndex = PharmaceuticalManager.Instance.currentPharmaceuticalIndex;

        foreach (PharmaceuticalItemUI pharmaceuticalItemUI in pharmaceuticalItems) {
            Pharmaceutical pharmaceutical = pharmaceuticalItemUI.pharmaceutical;
            PharmaceuticalManager.Instance.RefreshPharmaceutical(pharmaceutical.index);
            pharmaceuticalItemUI.SetActive(pharmaceutical.isActive);
            pharmaceuticalItemUI.SetAttention(pharmaceutical.hasAttention);
        }

        PharmaceuticalManager.Instance.RefreshPharmaceutical(currentSelectedIndex);
    }

    private void SetSelectedItem(int index) {
        if (currentSelectedItem != null)
            currentSelectedItem.SetVisual(false);
        currentSelectedItem = pharmaceuticalItems[index];
        currentSelectedItem.SetVisual(true);
    }

    private void OnEnable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged += SetSelectedItem;

        StartCoroutine(Display());
    }

    private void OnDisable() {
        PharmaceuticalManager.Instance.pharmaceuticalChanged -= SetSelectedItem;

    }
}
