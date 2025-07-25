using System.Linq;
using UnityEngine;

public class MeleeTower : Tower
{
    protected override void Update()
    {
        currentFiringTime += Time.deltaTime;
        if (currentTargettingMode == TargettingModes.Focused && currentFiringTime >= stats.fireRate || currentFiringTime >= stats.fireRate * stats.scatteredFireRateModifier)
        {
            if (enemiesInRange.Count > 0)
            {
                foreach (Enemy foundEnemy in enemiesInRange.ToList())
                {
                    if (!foundEnemy || foundEnemy.isDying)
                    {
                        enemiesInRange.Remove(foundEnemy);
                    }
                    if (currentTargettingMode == TargettingModes.Focused)
                    {
                        foundEnemy.TakeDamage(stats.damage);
                    }
                    else
                    {
                        foundEnemy.TakeDamage(stats.damage * stats.scatteredDamageModifier);
                    }
                    currentFiringTime = 0;
                }
                if (animator)
                {
                    animator.Play("AttackAnimation");
                }   
            }
        }
    }

}
