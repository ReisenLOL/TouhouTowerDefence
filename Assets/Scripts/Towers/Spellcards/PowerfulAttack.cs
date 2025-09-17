using UnityEngine;

public class PowerfulAttack : Spellcard
{
    public Projectile projectile;
    public float attackBaseDamage;
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
        Projectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
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
