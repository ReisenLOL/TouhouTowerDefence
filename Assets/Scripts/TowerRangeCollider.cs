using System;
using UnityEngine;

public class TowerRangeCollider : MonoBehaviour
{
    public Tower thisTower;
    public GameObject showRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy foundEnemy = other.GetComponent<Enemy>();
        if (!foundEnemy.isAir || foundEnemy.isAir && thisTower.canDetectAir)
        {
            thisTower.enemiesInRange.Add(foundEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Enemy foundEnemy = other.GetComponent<Enemy>();
        if (thisTower.enemiesInRange.Contains(foundEnemy))
        {
            thisTower.enemiesInRange.Remove(foundEnemy);
        }
    }
}
