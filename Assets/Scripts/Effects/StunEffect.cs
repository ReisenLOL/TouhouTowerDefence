using UnityEngine;

[CreateAssetMenu(fileName = "New Stun Effect", menuName = "Effects/StunEffect")]
public class StunEffect : Effect
{
    public override void ApplyEffects(Unit affectedUnit)
    {
        base.ApplyEffects(affectedUnit);
        affectedUnit.canMove = false;
        affectedUnit.canFire = false;
    }

    public override void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        base.RemoveEffect(effectInstanceToRemove);
        if (effectInstanceToRemove)
        {
            effectInstanceToRemove.affectedUnit.canMove = true;
            effectInstanceToRemove.affectedUnit.canFire = true;
            Destroy(effectInstanceToRemove);
        }
    }
}
