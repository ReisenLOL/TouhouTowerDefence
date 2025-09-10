using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public List<Tower> towersInRange;
    public Tower closestTower;
    public MoveProjectile projectile;

    public float range;
    protected override void Start()
    {
        base.Start();
        GameObject newRange = new GameObject("Enemy Range", typeof(CircleCollider2D), typeof(RangedEnemyRangeCollider));
        newRange.transform.SetParent(transform);
        newRange.transform.position = transform.position;
        newRange.layer = 11;
        CircleCollider2D newRangeCollider = newRange.GetComponent<CircleCollider2D>();
        newRangeCollider.radius = range;
        newRangeCollider.isTrigger = true;
    }
    protected override void Update()
    {
        base.Update();
        currentFiringTime += Time.deltaTime;
        if (canFire && !isDying && currentFiringTime >= fireRate)
        {
            foreach (Tower foundTower in towersInRange.ToList())
            {
                if (!foundTower)
                {
                    towersInRange.Remove(foundTower);
                    continue;
                }

                if (!closestTower || Vector3.Distance(transform.position, foundTower.transform.position) < Vector3.Distance(transform.position, closestTower.transform.position))
                {
                    closestTower = foundTower;
                }

                if (closestTower.TryGetComponent(out MeleeTower isMelee))
                {
                    FireProjectile(isMelee.blockingCollision.transform);
                }
                else
                {
                    FireProjectile(closestTower.transform);
                }
                currentFiringTime = 0;
            }
        }
    }
    private void FireProjectile(Transform direction)
    {
        MoveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.damage = damage * attackModifier;
        newProjectile.tag = gameObject.tag;
        newProjectile.RotateToTarget(direction.position);
        newProjectile.target = direction;
    }
}
