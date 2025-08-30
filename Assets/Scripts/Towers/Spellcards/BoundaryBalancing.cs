using System;
using UnityEngine;

public class BoundaryBalancing : Spellcard
{
    public Effect towerBuffEffect;
    public Effect enemyDebuffEffect;
    protected override void SpellCardEffects()
    {
        foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            enemyDebuffEffect.ApplyEffects(enemy);
        }

        foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsSortMode.None))
        {
            towerBuffEffect.ApplyEffects(tower);
        }
    }
}