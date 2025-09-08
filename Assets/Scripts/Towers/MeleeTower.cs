using System.Linq;
using UnityEngine;

public class MeleeTower : Tower
{
    public TowerBlockingCollision blockingCollision;
    protected override void Attack()
    {
        if (enemiesInRange.Count > 0)
        {
            if (animator)
            {
                animator.SetTrigger(attackAnimParam);
            }
        }
    }

    public void DealDamage()
    {
        foreach (Enemy foundEnemy in enemiesInRange.ToList())
        {
            if (!foundEnemy || foundEnemy.isDying)
            {
                enemiesInRange.Remove(foundEnemy);
                continue;
            }
            if (currentTargettingMode == TargettingModes.Focused)
            {
                foundEnemy.TakeDamage(stats.damage);
            }
            else
            {
                foundEnemy.TakeDamage(stats.damage * stats.scatteredDamageModifier);
            }
        }
    }
}
