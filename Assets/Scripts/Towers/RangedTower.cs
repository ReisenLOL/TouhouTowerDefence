using System.Linq;
using UnityEngine;

public class RangedTower : Tower
{
    public MoveProjectile projectile;

    protected override void Update()
    {
        currentFiringTime += Time.deltaTime;
        if (currentFiringTime >= fireRate)
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
                    closestEnemy.TakeDamage(damage);
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
