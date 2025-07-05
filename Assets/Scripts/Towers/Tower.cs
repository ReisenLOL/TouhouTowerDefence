using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    // [stats]
    public float range; //change this to a range shape later
    public float damage;
    public float fireRate;

    
    // [placement stuff]
    public bool isPlaced;
    
    // [identification]
    public string towerID;
    
    protected override void OnKill()
    {
        List<TowerToPlace> allTowers = FindFirstObjectByType<TowerPlacement>().availableTowers;
        foreach (TowerToPlace towerToPlace in allTowers)
        {
            if (towerToPlace.tower == this)
            {
                towerToPlace.onCooldown = true;
                break;
            }
        }
        Destroy(gameObject);
    }
}
