using System.Collections.Generic;
using UnityEngine;

public class LunaricRedEyes : Spellcard
{
    // change the name of this spellcard later lol
    public float stunDuration;
    public float currentStunTime;
    private List<Enemy> stunnedEnemies;
    private bool stunActive;
    protected override void SpellCardEffects()
    {
        stunnedEnemies = thisTower.enemiesInRange;
        Debug.Log("yeah");
        stunActive = true;
        foreach (Enemy enemy in stunnedEnemies)
        {
            enemy.canMove = false;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (stunActive)
        {
            currentStunTime += Time.deltaTime;
            if (currentStunTime > stunDuration)
            {
                foreach (Enemy enemy in stunnedEnemies)
                {
                    if (enemy)
                    {
                        enemy.canMove = true;
                    }
                }
                stunnedEnemies.Clear();
                stunActive = false;
                currentStunTime = 0;
            }
        }
    }
}
