using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Header("[CACHE]")] 
    public AudioClip attackSound;
    public AudioClip deathSound;
    public float attackSoundVolume;
    public AudioSource audioSource;
    public Animator animator;
    public List<Enemy> enemiesInRange = new();
    public List<Enemy> currentlyBlocking = new();
    public Enemy closestEnemy;
    protected float currentFiringTime;
    private Transform healthBarUI;
    public List<Spellcard> spellcardList;
    public string attackAnimParam;
    public string deployAnimParam;
    protected override void Start()
    {
        base.Start();
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
        audioSource.PlayOneShot(deathSound);
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
        TryAttack();
    }

    protected virtual void TryAttack()
    {
        currentFiringTime += Time.deltaTime;
        if (currentTargettingMode == TargettingModes.Focused && currentFiringTime >= stats.fireRate || currentFiringTime >= stats.fireRate * stats.scatteredFireRateModifier)
        {
            Attack();
            currentFiringTime = 0;
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
