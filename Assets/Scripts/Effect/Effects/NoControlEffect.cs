using UnityEngine;

[CreateAssetMenu(fileName = "NoControlEffect", menuName = "Effect/NoControlEffect")]
public class NoControlEffect : Effect {
    public override void ApplyEffect(PlayerEffectHandler playerEffectHandler) {
        playerEffectHandler.ChangeControl(false);
    }
}
