using System;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private Tower adjacentTower;
    protected override void Update()
    {
        base.Update();
        currentFiringTime += Time.deltaTime;
        if (currentFiringTime >= fireRate)
        {
            if (adjacentTower && !canMove)
            {
                currentFiringTime = 0;
                adjacentTower.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        adjacentTower = other.gameObject.GetComponentInParent<Tower>();
    }

    private void OnTriggerExit(Collider other)
    {
        adjacentTower = null;
    }
}
