using System;
using UnityEngine;
// ReSharper disable All

public class TowerToPlace : MonoBehaviour
{
    public Tower tower;
    public Sprite portrait;
    public bool isPlaced;
    public int cost;
    public float cooldown;
    public bool onCooldown;
    public float currentCooldownTimer;

    private void Start()
    {
        currentCooldownTimer = cooldown;
    }

    private void Update()
    {
        if (onCooldown)
        {
            currentCooldownTimer -= Time.deltaTime;
            if (currentCooldownTimer <= 0)
            {
                onCooldown = false;
                currentCooldownTimer = cooldown;
            }
        }
    }
}
