using System;
using UnityEngine;

public class Spellcard : MonoBehaviour
{
    [Header("[IDENTIFICATION]")]
    public string spellcardID;
    public Sprite spellcardImage;
    public string spellcardDescription;
    [Header("[STATS]")]
    public float cooldown;
    public float currentCooldownTime;
    public bool onCooldown;
    private ShowTowerInfo towerInfoUI;
    [Header("[CACHE]")]
    public Tower thisTower;

    protected virtual void Start()
    {
        towerInfoUI = FindFirstObjectByType<ShowTowerInfo>();
    }

    public void CastSpellCard()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            SpellCardEffects();
            currentCooldownTime = cooldown;
            towerInfoUI.RebuildSpellcardList();
        }
    }

    protected virtual void SpellCardEffects()
    {
        
    }
    protected virtual void Update()
    {
        if (onCooldown)
        {
            currentCooldownTime -= Time.deltaTime;
            if (currentCooldownTime <= 0)
            {
                onCooldown = false;
            }   
        }
    }
}
