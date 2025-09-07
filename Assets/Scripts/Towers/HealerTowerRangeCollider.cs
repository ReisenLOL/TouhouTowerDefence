using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerTowerRangeCollider : TowerRangeCollider
{
    private HealerTower thisHealerTower;
    public Transform[] allPlacementTiles;
    public LayerMask towerLayer;
    public bool updateTowerPlacements;
    public List<Collider2D> towersFound = new();
    private void Start()
    {
        thisHealerTower = thisTower.GetComponent<HealerTower>();
    }

    private void Update()
    {
        if (updateTowerPlacements)
        {
            towersFound.Clear();
            foreach (Transform tilecheck in allPlacementTiles)
            {
                Collider2D foundTower = FindTowerOnTile(tilecheck);
                if (foundTower)
                {
                    towersFound.Add(foundTower);
                }
            }
            foreach (Collider2D tower in towersFound.ToList())
            {
                if (!tower)
                {
                    towersFound.Remove(tower);
                }
                if (tower.TryGetComponent(out Tower isRangedTower))
                {
                    thisHealerTower.towersInRange.Add(isRangedTower);
                }
                else if (tower.TryGetComponent(out TowerBlockingCollision isMeleeTower))
                {
                    thisHealerTower.towersInRange.Add(isMeleeTower.thisTower);
                }
            }
            updateTowerPlacements = false;
        }
    }
    private Collider2D FindTowerOnTile(Transform tileCheck)
    {
        return Physics2D.OverlapCircle(tileCheck.position, 0.05f, towerLayer);
    }
}
