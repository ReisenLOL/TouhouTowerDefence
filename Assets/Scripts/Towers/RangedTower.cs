using System.Linq;
using UnityEngine;

public class RangedTower : Tower
{
    public Projectile projectile;
    public RangedTowerSprite thisSpriteScript;

    protected override void Attack()
    {
        foreach (Enemy foundEnemy in enemiesInRange.ToList())
        {
            if (!foundEnemy || foundEnemy.isDying)
            {
                enemiesInRange.Remove(foundEnemy);
                continue;
            }
            if (!closestEnemy || Vector3.Distance(transform.position, foundEnemy.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
            {
                closestEnemy = foundEnemy;
            }
            if (closestEnemy)
            {
                thisSpriteScript.target = closestEnemy.transform;
                if (animator)
                {
                    animator.SetTrigger(attackAnimParam);
                }
                else
                {
                    FireProjectile(closestEnemy.transform);
                    audioSource.PlayOneShot(attackSound, attackSoundVolume);
                }
                currentFiringTime = 0;   
            }
        }
    }
    public virtual void FireProjectile(Transform direction)
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
    }
}
