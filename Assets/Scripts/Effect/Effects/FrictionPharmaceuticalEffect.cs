using UnityEngine;

[CreateAssetMenu(fileName = "FrictionPharmaceuticalEffect", menuName = "Effect/FrictionPharmaceuticalEffect")]
public class FrictionPharmaceuticalEffect : Pharmaceutical {
    [SerializeField] float increaseFactor;

    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        PlayerInfor playerInfor = playerEffectHandler.playerInfor;
        float increaseEnergySpeed = playerInfor.DefaultEnergySpeed * increaseFactor;
        playerEffectHandler.ChangeEnergySpeed(increaseEnergySpeed);
    }
}
