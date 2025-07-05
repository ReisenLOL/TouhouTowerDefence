using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    // [identification]
    public string towerID;

    // [stats]
    public TowerRangeCollider range;
    public float damage;
    public float fireRate;
    public bool isCliffTower;
    public bool canDetectAir;
    public int blockAmount;
    
    // [cache]
    public List<Enemy> enemiesInRange = new();
    public List<Enemy> currentlyBlocking = new();
    
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

    private void OnDestroy()
    {
        foreach (Enemy enemy in currentlyBlocking)
        {
            enemy.canMove = true;
        }
    }
}
