using UnityEngine;

public enum EffectType {
    Internal, External
}

public enum EffectImpact {
    Beneficial, Neutral, Harmful
}

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Effect")]
public class Effect : ScriptableObject {
    [SerializeField] Sprite effectImage;
    [SerializeField] string infor;
    [SerializeField] int priority;
    [SerializeField] EffectType type;
    [SerializeField] EffectImpact impact;

    public Sprite EffectImage => effectImage;
    public string Infor => infor;
    public int Priority => priority;
    public EffectType Type => type;
    public EffectImpact Impact => impact;

    public virtual void ApplyEffect(PlayerEffectHandler playerEffectHandler) { }
}