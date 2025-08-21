using System.Collections.Generic;
using UnityEngine;

public class HealerTower : RangedTower
{
    public List<Tower> towersInRange;
    protected Tower closestTower; 
    protected override void TryAttack()
    {
        currentFiringTime += Time.deltaTime;
        if (currentTargettingMode == TargettingModes.Focused && currentFiringTime >= stats.fireRate || currentFiringTime >= stats.fireRate * stats.scatteredFireRateModifier)
        {
            foreach (Tower foundTower in towersInRange)
            {
                if (!foundTower || foundTower.isDying)
                {
                    towersInRange.Remove(foundTower);
                    continue;
                }
                if (!closestTower || Vector3.Distance(transform.position, foundTower.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestTower = foundTower;
                }

                if (closestTower)
                {
                    if (currentTargettingMode == TargettingModes.Focused)
                    {
                        closestTower.HealDamage(stats.damage);
                    }
                    else
                    {
                        closestTower.HealDamage(stats.damage * stats.scatteredDamageModifier);
                    }
                    audioSource.PlayOneShot(attackSound);
                    FireProjectile(closestTower.transform);
                    currentFiringTime = 0;   
                }
            }
        }
    }
}
