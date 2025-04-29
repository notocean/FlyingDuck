using UnityEngine;

[CreateAssetMenu(fileName = "ImmunePharmaceuticalEffect", menuName = "Effect/ImmunePharmaceuticalEffect")]
public class ImmunePharmaceuticalEffect : Pharmaceutical {
    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        playerEffectHandler.ChangeImmune(true);
    }
}
