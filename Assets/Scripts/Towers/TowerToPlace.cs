using System;
using UnityEngine;
// ReSharper disable All

public class TowerToPlace : MonoBehaviour
{
    public Tower tower;
    public int cost;
    public float cooldown;
    public bool onCooldown;
    private float currentCooldownTimer;

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
