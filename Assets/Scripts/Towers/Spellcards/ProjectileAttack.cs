using UnityEngine;

public class ProjectileAttack : Spellcard
{
    public Projectile projectile;
    public Transform rangeToCheck;
    public float attackBaseDamage;

    protected override void Start()
    {
        base.Start();
        rangeToCheck = thisTower.GetComponentInChildren<TowerRangeCollider>().transform;
    }
    protected override void SpellCardEffects()
    {
        FireProjectile();
    }
    public void FireProjectile()
    {
        Projectile newProjectile = Instantiate(projectile, rangeToCheck);
        newProjectile.transform.position = thisTower.transform.position;
        if (thisTower.currentTargettingMode == Tower.TargettingModes.Focused)
        {
            newProjectile.damage = attackBaseDamage * thisTower.attackModifier;
        }
        else
        {
            newProjectile.damage = attackBaseDamage * thisTower.attackModifier * thisTower.stats.scatteredDamageModifier;
        }
    }
}
