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

    private void Start()
    {
        towerInfoUI = FindFirstObjectByType<ShowTowerInfo>();
    }

    public void CastSpellCard()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            SpellCardEffects();
            towerInfoUI.RebuildSpellcardList();
        }
    }

    protected virtual void SpellCardEffects()
    {
        
    }
    private void Update()
    {
        if (onCooldown)
        {
            currentCooldownTime += Time.deltaTime;
            if (currentCooldownTime >= cooldown)
            {
                currentCooldownTime = 0;
                onCooldown = false;
            }   
        }
    }
}
