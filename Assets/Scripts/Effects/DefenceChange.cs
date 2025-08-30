using UnityEngine;

[CreateAssetMenu(fileName = "New Defence effect", menuName = "Effects/DefenseEffect")]
public class DefenceChange : Effect
{
    [Header("This is a delta. And for the stupid person named Sylvia (me), that means it increments, not setting the entire value.")]
    public float defenseDeltaChangeValue;
    public override void ApplyEffects(Unit affectedUnit)
    {
        base.ApplyEffects(affectedUnit);
        affectedUnit.defence += defenseDeltaChangeValue;
    }

    public override void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        base.RemoveEffect(effectInstanceToRemove);
        effectInstanceToRemove.affectedUnit.defence -= defenseDeltaChangeValue;
        Destroy(effectInstanceToRemove);
    }
}
