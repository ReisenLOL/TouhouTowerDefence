using System;
using UnityEngine;

public class Spellcard : MonoBehaviour
{
    public float cooldown;
    public float currentCooldownTime;
    public bool canCast;
    public void CastSpellCard()
    {
        if (canCast)
        {
            canCast = false;
            SpellCardEffects();
        }
    }

    protected virtual void SpellCardEffects()
    {
        
    }
    private void Update()
    {
        if (!canCast)
        {
            currentCooldownTime += Time.deltaTime;
            if (currentCooldownTime >= cooldown)
            {
                currentCooldownTime = 0;
                canCast = true;
            }   
        }
    }
}
