using System;
using UnityEngine;

public class Enemy : Unit
{
    [Header("[STATS]")]
    public float damage;
    public float fireRate;
    public float moveSpeed;
    public bool isAir;
    
    [Header("[CACHE]")]
    public Rigidbody2D rb;
    public int currentWaypoint;
    public WaveManager.SpawnAndMovement selectedSpawnAndMovement;

    public Vector2 moveLocation;
    protected float currentFiringTime;
    private Animator animator;
    private GameManager gameManager;
    public string deathAnimParamName;
    public string speedAnimParamName;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            moveLocation = (selectedSpawnAndMovement.waypoints[currentWaypoint].transform.position - transform.position).normalized;
            if (animator)
            {
                animator.SetFloat(speedAnimParamName, 1f);
            }
        }
        else if (!canMove && animator)
        {
            animator.SetFloat(speedAnimParamName, 0f);
        }

        if (isDying && currentState == 0)
        {
            StartCoroutine(DeathAnimation());
        }
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveLocation * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    protected override void OnKill()
    {
        canMove = false;
        gameManager.currentEnemyCount++;
        gameManager.UpdateEnemyCount();
        if (animator)
        {
            animator.SetBool(deathAnimParamName, true);
        }
    }
}
