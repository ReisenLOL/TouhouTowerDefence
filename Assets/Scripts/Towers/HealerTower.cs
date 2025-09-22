using System.Collections.Generic;
using UnityEngine;

public class HealerTower : RangedTower
{
    public HealerTowerSprite thisTowerSprite;
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
                if ((!closestTower || foundTower.health < closestTower.health && foundTower) && foundTower.health < foundTower.maxHealth)
                {
                    closestTower = foundTower;
                }

                if (closestTower && closestTower.health < closestTower.maxHealth)
                {
                    if (animator)
                    {
                        thisTowerSprite.healingTarget = closestTower;
                        animator.SetTrigger(attackAnimParam);
                    }
                    else if (currentTargettingMode == TargettingModes.Focused)
                    {
                        closestTower.HealDamage(stats.damage);
                    }
                    else
                    {
                        closestTower.HealDamage(stats.damage * stats.scatteredDamageModifier); //??
                    }
                    audioSource.PlayOneShot(attackSound);
                    
                    //FireProjectile(closestTower.transform);
                    currentFiringTime = 0;   
                }
            }
        }
    }
}
