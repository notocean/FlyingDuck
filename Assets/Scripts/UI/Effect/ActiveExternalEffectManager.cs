using UnityEngine;

public class ActiveExternalEffectManager : ActiveEffectManager {
    protected override void Awake() {
        base.Awake();
        playerEffectHandler.externalEffectChanged += EffectChangeHandle;
    }

    protected override void AddEffectUI(Effect effect) {
        if (!activeEffectObjects.ContainsKey(effect)) {
            GameObject newEffectUI = Instantiate(activeEffectPrefab, transform);

            EffectShower effectInformation = newEffectUI.GetComponent<EffectShower>();
            string style = "";
            switch (effect.Impact) {
                case EffectImpact.Beneficial:
                    style = beneficialNameStyle;
                    break;
                case EffectImpact.Neutral:
                    style = neutralNameStyle;
                    break;
                case EffectImpact.Harmful:
                    style = harmfulNameStyle;
                    break;
            }

            effectInformation.Initial(effect.EffectImage, $"{style}{effect.Infor}");
            activeEffectObjects[effect] = newEffectUI;
        }
    }
}
