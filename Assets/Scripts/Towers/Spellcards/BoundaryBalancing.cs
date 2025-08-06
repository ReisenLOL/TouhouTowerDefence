using System;
using UnityEngine;

public class BoundaryBalancing : Spellcard
{
    public float buffDuration;
    public float currentbuffTime;
    public bool buffActive;
    protected override void SpellCardEffects()
    {
        buffActive = true;
        foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            enemy.defence = 0.8f;
        }

        foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsSortMode.None))
        {
            tower.defence = 1.2f;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (buffActive)
        {
            currentbuffTime += Time.deltaTime;
            if (currentbuffTime > buffDuration)
            {
                foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                {
                    enemy.defence = 1f;
                }

                foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsSortMode.None))
                {
                    tower.defence = 1f;
                }
                currentbuffTime = 0;
                buffActive = false;
            }
        }
    }
}