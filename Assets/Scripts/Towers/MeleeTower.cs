using System.Linq;
using UnityEngine;

public class MeleeTower : Tower
{
    protected override void Update()
    {
        currentFiringTime += Time.deltaTime;
        if (currentFiringTime >= fireRate)
        {
            if (enemiesInRange.Count > 0)
            {
                foreach (Enemy foundEnemy in enemiesInRange.ToList())
                {
                    if (!foundEnemy || foundEnemy.isDying)
                    {
                        enemiesInRange.Remove(foundEnemy);
                    }
                    foundEnemy.TakeDamage(damage);
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
