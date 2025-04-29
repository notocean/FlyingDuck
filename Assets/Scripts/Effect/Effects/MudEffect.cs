using UnityEngine;

[CreateAssetMenu(fileName = "MudEffect", menuName = "Effect/MudEffect")]
public class MudEffect : Effect {
    [SerializeField] float decreaseEnergySpeed;
    [SerializeField] float decreaseWalkSpeed;
    [SerializeField] float decreaseFlyForce;

    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        PlayerInfor playerInfor = playerEffectHandler.playerInfor;
        float energySpeed = - playerInfor.DefaultEnergySpeed * decreaseEnergySpeed;
        float walkSpeed = - playerInfor.DefaultWalkSpeed * decreaseWalkSpeed;
        float flyForce = - playerInfor.DefaultFlyForce * decreaseFlyForce;
        playerEffectHandler.ChangeEnergySpeed(energySpeed);
        playerEffectHandler.ChangeWalkSpeed(walkSpeed);
        playerEffectHandler.ChangeFlyForce(flyForce);
    }
}
