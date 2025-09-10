using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tower : Unit
{
    [Header("[IDENTIFICATION]")]
    public string towerID;
    
    [Header("[STATS]")]
    public TowerRangeCollider focusedRange;

    public TowerRangeCollider scatteredRange;
    public enum TargettingModes {Focused, Scattered}
    public TargettingModes currentTargettingMode = TargettingModes.Focused;
    public TowerStats stats;
    public List<Spellcard> spellcardsToAdd = new();
    public bool isOneUse;

    [Header("[CACHE]")] 
    public AudioClip attackSound;
    public AudioClip deathSound;
    public float attackSoundVolume;
    public AudioSource audioSource;
    public Animator animator;
    public List<Enemy> enemiesInRange = new();
    public List<Enemy> currentlyBlocking = new();
    public Enemy closestEnemy = null;
    protected float currentFiringTime;
    private Transform healthBarUI;
    public List<Spellcard> spellcardList;
    public string attackAnimParam;
    public string deployAnimParam;
    public ShowTowerInfo towerInfoUI;
    protected override void Start()
    {
        base.Start();
        towerInfoUI = FindFirstObjectByType<ShowTowerInfo>();
        audioSource = FindFirstObjectByType<AudioSource>();
        healthBarUI = transform.Find("HealthBarUI").Find("HealthBarPanelBG").Find("HealthBar").transform;
        animator = GetComponentInChildren<Animator>();
        foreach (Spellcard spellcard in spellcardsToAdd)
        {
            Spellcard newSpellcard = Instantiate(spellcard, transform);
            newSpellcard.thisTower = this;
            spellcardList.Add(newSpellcard);
        }
    }

    [ContextMenu("Force Kill")]
    protected override void OnKill()
    {
        TowerPlacement towerPlacement = FindFirstObjectByType<TowerPlacement>();
        if (!isOneUse)
        {
            List<TowerToPlace> allTowers = towerPlacement.availableTowers;
            foreach (TowerToPlace towerToPlace in allTowers)
            {
                if (towerToPlace.tower.towerID == towerID)
                {
                    towerToPlace.isPlaced = false;
                    towerToPlace.onCooldown = true;
                    towerPlacement.RebuildTowerSelection();
                    break;
                }
            }
        }
        else
        {
            foreach (TowerToPlace towerToPlace in towerPlacement.availableTowers)
            {
                if (towerToPlace.tower.towerID == towerID)
                {
                    towerPlacement.availableTowers.Remove(towerToPlace);
                    break;
                }
            }
        }
        audioSource.PlayOneShot(deathSound);
    }

    public override void TakeDamage(float damageTaken)
    {
        base.TakeDamage(damageTaken);
        UpdateHealthBar();
    }

    public override void HealDamage(float healing)
    {
        base.HealDamage(healing);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarUI.localScale = new Vector3(health/maxHealth, healthBarUI.localScale.y);
        if (towerInfoUI.selectedTower == this)
        {
            towerInfoUI.UpdateTowerHealthBar();
        }
    }
    protected virtual void Update()
    {
        //since were having like different tower classes, the update function might be different on each one.
        TryAttack();
    }

    protected virtual void TryAttack()
    {
        if (canFire)
        {
            currentFiringTime += Time.deltaTime;
            if (currentTargettingMode == TargettingModes.Focused && currentFiringTime >= stats.fireRate || currentFiringTime >= stats.fireRate * stats.scatteredFireRateModifier)
            {
                Attack();
                currentFiringTime = 0;
            }
        }
    }

    protected virtual void Attack()
    {
        //this is example function, never actually used.
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
            closestEnemy.TakeDamage(stats.damage);
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
