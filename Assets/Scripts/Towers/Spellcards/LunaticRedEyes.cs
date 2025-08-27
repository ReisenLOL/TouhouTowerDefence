using System.Collections.Generic;
using UnityEngine;

public class LunaticRedEyes : Spellcard
{
    // change the name of this spellcard later lol
    private List<Enemy> stunnedEnemies;
    public Effect stunEffect;
    protected override void SpellCardEffects()
    {
        stunnedEnemies = thisTower.enemiesInRange;
        foreach (Enemy enemy in stunnedEnemies)
        {
            stunEffect.ApplyEffects(enemy);
        }
    }
}
