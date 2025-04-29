using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInternalEffectManager : ActiveEffectManager
{
    protected override void Awake() {
        base.Awake();
        StartCoroutine(WaitForEndStart());
        playerEffectHandler.internalEffectChanged += EffectChangeHandle;
    }

    IEnumerator WaitForEndStart() {
        // Chờ đến frame tiếp theo
        yield return null;
        Initial();
    }

    private void Initial() {
        foreach (Pharmaceutical pharmaceutical in PharmaceuticalManager.Instance.pharmaceuticalList) {
            if (pharmaceutical.timeRemainingList[GameManager.Instance.LevelIndex - 1] != 0) {
                playerEffectHandler.AddEffect(pharmaceutical);
            }
        }
    }

    protected override void AddEffectUI(Effect effect) {
        Pharmaceutical pharmaceutical = effect as Pharmaceutical;
        if (!activeEffectObjects.ContainsKey(pharmaceutical)) {
            GameObject newEffectUI = Instantiate(activeEffectPrefab, transform);

            EffectShower effectInformation = newEffectUI.GetComponent<EffectShower>();
            string style = "";
            switch (pharmaceutical.Impact) {
                case EffectImpact.Beneficial:
                    style = beneficialNameStyle;
                    break;
                case EffectImpact.Neutral:
                    style = neutralNameStyle;
                    break;
            }

            effectInformation.Initial(pharmaceutical.EffectImage, $"{style}{pharmaceutical.Infor}");

            UsedPharmaceutical usedPharmaceutical = newEffectUI.GetComponent<UsedPharmaceutical>();
            usedPharmaceutical.Initial(playerEffectHandler, pharmaceutical, pharmaceutical.timeRemainingList[GameManager.Instance.LevelIndex - 1]);

            activeEffectObjects[pharmaceutical] = newEffectUI;
        }
    }
}
