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
    public bool canMove = true;
    public bool isDying;
    public Vector2 moveLocation;
    protected float currentFiringTime;
    private Animator animator;
    private GameManager gameManager;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            moveLocation = (selectedSpawnAndMovement.waypoints[currentWaypoint].transform.position - transform.position).normalized;
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
        isDying = true;
        gameManager.currentEnemyCount++;
        gameManager.UpdateEnemyCountUI();
        if (animator)
        {
            animator.Play("Death Animation");
            Destroy(gameObject, 1.6f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
