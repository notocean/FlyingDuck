using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedPharmaceuticalManager : MonoBehaviour
{
    [SerializeField] GameObject usedPharmaceuticalPrefab;

    private void Start() {
        Initial();
    }

    private void Initial() {
        foreach (Pharmaceutical pharmaceutical in PharmaceuticalList.Instance.pharmaceuticals) {
            if (pharmaceutical.timeRemaining != 0) {
                UsePharmaceuticalHandle(pharmaceutical.index, pharmaceutical.timeRemaining);
            }
        }
    }

    private void UsePharmaceuticalHandle(int index, float effectTime) {
        UsedPharmaceutical usedPharmaceutical = Instantiate(usedPharmaceuticalPrefab, transform).GetComponent<UsedPharmaceutical>();
        usedPharmaceutical.Initial(index, effectTime);
    }

    private void OnEnable() {
        PharmaceuticalList.Instance.usePharmaceuticalEvent.AddListener(UsePharmaceuticalHandle);
    }

    private void OnDisable() {
        PharmaceuticalList.Instance.usePharmaceuticalEvent.RemoveListener(UsePharmaceuticalHandle);
    }
}
