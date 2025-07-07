using System;
using UnityEngine;

public class RangedEnemyRangeCollider : MonoBehaviour
{
    public RangedEnemy thisEnemy;
    private void Start()
    {
        thisEnemy = GetComponentInParent<RangedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        thisEnemy.towersInRange.Add(other.GetComponentInParent<Tower>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Tower foundTower = other.GetComponentInParent<Tower>();
        if (thisEnemy.towersInRange.Contains(foundTower))
        {
            thisEnemy.towersInRange.Remove(foundTower);   
        }
    }
}
