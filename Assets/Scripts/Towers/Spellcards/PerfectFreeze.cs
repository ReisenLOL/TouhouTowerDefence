using System.Collections.Generic;
using UnityEngine;

public class PerfectFreeze : Spellcard
{
    private List<Enemy> frozenEnemies;
    public Effect freezeEffect;
    protected override void SpellCardEffects()
    {
        frozenEnemies = thisTower.enemiesInRange;
        foreach (Enemy enemy in frozenEnemies)
        {
            freezeEffect.ApplyEffects(enemy);
        }
    }
}
