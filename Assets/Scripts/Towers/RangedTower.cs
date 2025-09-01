using System.Linq;
using UnityEngine;

public class RangedTower : Tower
{
    public Projectile projectile;

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
                if (animator)
                {
                    animator.SetTrigger(attackAnimParam);
                }
                else
                {
                    FireProjectile(closestEnemy.transform);
                    audioSource.PlayOneShot(attackSound, attackSoundVolume);
                }
                if (currentTargettingMode == TargettingModes.Focused)
                {
                    closestEnemy.TakeDamage(stats.damage);
                }
                else
                {
                    closestEnemy.TakeDamage(stats.damage * stats.scatteredDamageModifier);
                }
                currentFiringTime = 0;   
            }
        }
    }
    public void FireProjectile(Transform direction)
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.target = direction;
        newProjectile.RotateToTarget(direction.position);
    }
}
