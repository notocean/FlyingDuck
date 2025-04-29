using UnityEngine;

[CreateAssetMenu(fileName = "HoldEffect", menuName = "Effect/HoldEffect")]
public class HoldEffect : Effect
{
    [SerializeField] float holdTime;
    [SerializeField] float refreshTime;

    public float HoldTime => holdTime;
    public float RefreshTime => refreshTime;

    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        playerEffectHandler.ChangeControl(false);
    }
}
