using UnityEngine;

[CreateAssetMenu(fileName = "OngroundEffect", menuName = "Effect/OngroundEffect")]
public class OngroundEffect : Effect
{
    [SerializeField] int increaseEnergySpeedFactor;

    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        float value = increaseEnergySpeedFactor * playerEffectHandler.playerInfor.DefaultEnergySpeed;
        playerEffectHandler.ChangeEnergySpeed(value);
    }
}
