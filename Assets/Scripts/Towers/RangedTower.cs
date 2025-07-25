using System.Linq;
using UnityEngine;

public class RangedTower : Tower
{
    public MoveProjectile projectile;
    protected override void Update()
    {
        currentFiringTime += Time.deltaTime;
        if (currentTargettingMode == TargettingModes.Focused && currentFiringTime >= stats.fireRate || currentFiringTime >= stats.fireRate * stats.scatteredFireRateModifier)
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
                    if (currentTargettingMode == TargettingModes.Focused)
                    {
                        closestEnemy.TakeDamage(stats.damage);
                    }
                    else
                    {
                        closestEnemy.TakeDamage(stats.damage * stats.scatteredDamageModifier);
                    }
                    FireProjectile(closestEnemy.transform.position);
                    currentFiringTime = 0;   
                }
            }
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        MoveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.RotateToTarget(direction);
    }
}
