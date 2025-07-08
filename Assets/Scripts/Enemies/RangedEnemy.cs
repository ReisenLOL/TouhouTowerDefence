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
        if (currentFiringTime >= fireRate)
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
                closestTower.TakeDamage(damage);
                FireProjectile(closestTower.transform.position);
                currentFiringTime = 0;
            }
        }
    }
    private void FireProjectile(Vector2 direction)
    {
        MoveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.RotateToTarget(direction);
    }
}
