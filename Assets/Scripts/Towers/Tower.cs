using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : Unit
{
    [Header("[IDENTIFICATION]")]
    public string towerID;
    
    [Header("[STATS]")]
    public TowerRangeCollider range;
    public float damage;
    public float fireRate;
    public int blockAmount;
    public bool isCliffTower;
    public bool canDetectAir;
    public List<Spellcard> spellcardList = new();

    [Header("[CACHE]")] 
    public Animator animator;
    public List<Enemy> enemiesInRange = new();
    public List<Enemy> currentlyBlocking = new();
    public Enemy closestEnemy;
    protected float currentFiringTime;
    private Transform healthBarUI;
    public ShowTowerInfo showTowerInfo;


    protected virtual void Start()
    {
        healthBarUI = transform.Find("HealthBarUI").Find("HealthBarPanelBG").Find("HealthBar").transform;
        showTowerInfo = FindFirstObjectByType<ShowTowerInfo>();
        animator = GetComponentInChildren<Animator>();
    }

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

    public override void TakeDamage(float damageTaken)
    {
        base.TakeDamage(damageTaken);
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthBarUI.localScale = new Vector3(health/maxHealth, healthBarUI.localScale.y);
    }
    protected virtual void Update()
    {
        //since were having like different tower classes, the update function might be different on each one.
        currentFiringTime += Time.deltaTime;
        if (currentFiringTime >= fireRate)
        {
            foreach (Enemy foundEnemy in enemiesInRange.ToList())
            {
                if (!foundEnemy)
                {
                    enemiesInRange.Remove(foundEnemy);
                    continue;
                }
                if (!closestEnemy || Vector3.Distance(transform.position, foundEnemy.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = foundEnemy;
                }
                closestEnemy.TakeDamage(damage);
                currentFiringTime = 0;
            }
        }
    }

    protected void OnDestroy()
    {
        foreach (Enemy enemy in currentlyBlocking)
        {
            enemy.canMove = true;
        }
    }
}
