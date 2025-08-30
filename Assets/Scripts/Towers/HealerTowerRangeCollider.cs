using System;
using UnityEngine;

public class HealerTowerRangeCollider : TowerRangeCollider
{
    private HealerTower thisHealerTower;
    private void Start()
    {
        thisHealerTower = thisTower.GetComponent<HealerTower>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tower foundTower = other.GetComponent<Tower>();
        thisHealerTower.towersInRange.Add(foundTower);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Tower foundTower = other.GetComponent<Tower>();
        if (thisHealerTower.towersInRange.Contains(foundTower))
        {
            thisHealerTower.towersInRange.Remove(foundTower);
        }
    }
}
