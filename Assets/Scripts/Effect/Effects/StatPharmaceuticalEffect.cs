using UnityEngine;

[CreateAssetMenu(fileName = "StatPharmaceuticalEffect", menuName = "Effect/StatPharmaceuticalEffect")]
public class StatPharmaceuticalEffect : Pharmaceutical {
    [SerializeField] float increaseFactor;

    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        PlayerInfor playerInfor = playerEffectHandler.playerInfor;
        float increaseWalkSpeed = playerInfor.DefaultWalkSpeed * increaseFactor;
        float increaseFlyForce = playerInfor.DefaultFlyForce * increaseFactor;
        playerEffectHandler.ChangeWalkSpeed(increaseWalkSpeed);
        playerEffectHandler.ChangeFlyForce(increaseFlyForce);
    }
}
