using UnityEngine;

[CreateAssetMenu(fileName = "New Freeze Effect", menuName = "Effects/PerfectFreeze")]
public class FreezeEffect : Effect
{
    public GameObject ice;
    public override void ApplyEffects(Unit affectedUnit)
    {
        base.ApplyEffects(affectedUnit);
        affectedUnit.canMove = false;
        affectedUnit.canFire = false;
        GameObject newIceObject = Instantiate(ice);
        newIceObject.transform.position = affectedUnit.transform.position;
        affectedUnit.GetComponent<EffectInstance>().effectVisuals.Add(newIceObject);
        Animator affectedUnitAnimator = affectedUnit.GetComponentInChildren<Animator>();
        if (affectedUnitAnimator)
        {
            affectedUnitAnimator.speed = 0f;
        }
    }

    public override void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        base.RemoveEffect(effectInstanceToRemove);
        if (effectInstanceToRemove)
        {
            effectInstanceToRemove.affectedUnit.canMove = true;
            effectInstanceToRemove.affectedUnit.canFire = true;
            Animator affectedUnitAnimator = effectInstanceToRemove.affectedUnit.GetComponentInChildren<Animator>();
            if (affectedUnitAnimator)
            {
                affectedUnitAnimator.speed = 1f;
            }
            Destroy(effectInstanceToRemove);
        }
    }
}
