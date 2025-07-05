using System;
using UnityEngine;

public class Enemy : Unit
{
    // [stats]
    public float damage;
    public float moveSpeed;
    
    // [movement]
    public Rigidbody2D rb;
    public int currentWaypoint;
    public WaveManager.SpawnAndMovement selectedSpawnAndMovement;
    public bool canMove = true;
    public Vector2 moveLocation;

    private void Update()
    {
        if (canMove)
        {
            moveLocation = (transform.position - selectedSpawnAndMovement.waypoints[currentWaypoint].transform.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveLocation * moveSpeed;
        }
    }
}
