using UnityEngine;

[CreateAssetMenu(fileName = "New Attack effect", menuName = "Effects/AttackEffect")]
public class AttackChange : Effect
{
    [Header("This is a delta. And for the stupid person named Sylvia (me), that means it increments, not setting the entire value.")]
    public float attackDeltaChangeValue;
    public override void ApplyEffects(Unit affectedUnit)
    {
        base.ApplyEffects(affectedUnit);
        affectedUnit.defence += attackDeltaChangeValue;
    }

    public override void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        base.RemoveEffect(effectInstanceToRemove);
        effectInstanceToRemove.affectedUnit.attackModifier -= attackDeltaChangeValue;
        Destroy(effectInstanceToRemove);
    }
}