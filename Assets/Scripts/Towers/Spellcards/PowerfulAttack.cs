using UnityEngine;

public class PowerfulAttack : Spellcard
{
    public ExplosiveProjectile projectile;
    public float attackBaseDamage;
    public float blastRadius;
    public float blastDamage;
    protected override void SpellCardEffects()
    {
        if (thisTower.closestEnemy)
        {
            FireProjectile(thisTower.closestEnemy.transform);
        }
        else
        {
            onCooldown = false;
            currentCooldownTime = 0;
        }
    }

    public void FireProjectile(Transform direction)
    {
        ExplosiveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.radius = blastRadius;
        newProjectile.blastDamage = blastDamage;
        if (thisTower.currentTargettingMode == Tower.TargettingModes.Focused)
        {
            newProjectile.damage = attackBaseDamage * thisTower.attackModifier;
        }
        else
        {
            newProjectile.damage = attackBaseDamage * thisTower.attackModifier * thisTower.stats.scatteredDamageModifier;
        }
        newProjectile.target = direction;
        newProjectile.RotateToTarget(direction.position);
    }
}
