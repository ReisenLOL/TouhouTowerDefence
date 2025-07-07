using System;
using UnityEngine;

public class Enemy : Unit
{
    [Header("[STATS]")]
    public float damage;
    public float fireRate;
    public float moveSpeed;
    public bool isAir;
    
    [Header("[MOVEMENT]")]
    public Rigidbody2D rb;
    public int currentWaypoint;
    public WaveManager.SpawnAndMovement selectedSpawnAndMovement;
    public bool canMove = true;
    public Vector2 moveLocation;

    // [CACHE]
    protected float currentFiringTime;
    private GameManager gameManager;

    protected virtual void Start()
    {
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
        Destroy(gameObject);
        gameManager.currentEnemyCount++;
        gameManager.UpdateEnemyCountUI();
    }
}
