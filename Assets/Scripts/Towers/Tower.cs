using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    // [identification]
    public string towerID;

    // [stats]
    public float range; //change this to a range shape later
    public float damage;
    public float fireRate;
    public bool isCliffTower;
    public int blockAmount;
    
    [ContextMenu("Force Kill")]
    protected override void OnKill()
    {
        List<TowerToPlace> allTowers = FindFirstObjectByType<TowerPlacement>().availableTowers;
        foreach (TowerToPlace towerToPlace in allTowers)
        {
            
            if (towerToPlace.tower.towerID == towerID)
            {
                towerToPlace.isPlaced = false;
                towerToPlace.onCooldown = true;
                FindFirstObjectByType<TowerPlacement>().RebuildTowerSelection();
                break;
            }
        }
        Destroy(gameObject);
    }
}
