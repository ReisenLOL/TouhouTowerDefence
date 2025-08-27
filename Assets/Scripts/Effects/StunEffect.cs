using UnityEngine;

[CreateAssetMenu(fileName = "New Stun Effect", menuName = "Effects/StunEffect")]
public class StunEffect : Effect
{
    public override void ApplyEffects(Unit affectedUnit)
    {
        base.ApplyEffects(affectedUnit);
        affectedUnit.canMove = false;
    }

    public override void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        base.RemoveEffect(effectInstanceToRemove);
        effectInstanceToRemove.affectedUnit.canMove = true;
        Destroy(effectInstanceToRemove);
    }
}
