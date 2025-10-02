using UnityEngine;

public class HeavyCasterTower : RangedTower
{
    public override void FireProjectile(Transform direction)
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        if (currentTargettingMode == TargettingModes.Focused)
        {
            newProjectile.damage = stats.damage * attackModifier;
        }
        else
        {
            newProjectile.damage = stats.damage * attackModifier * stats.scatteredDamageModifier;
        }
        newProjectile.target = direction;
        newProjectile.RotateToTarget(direction.position);
        newProjectile.willBypassDefense = true;
    }
}
