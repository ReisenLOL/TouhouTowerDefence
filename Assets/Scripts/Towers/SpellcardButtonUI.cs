using System;
using TMPro;
using UnityEngine;

public class SpellcardButtonUI : MonoBehaviour
{
    private GameObject overlay;
    private TextMeshProUGUI timer;
    public Spellcard thisSpellcard;
    public bool showTimer;

    private void Start()
    {
        overlay = transform.Find("CooldownPanel").gameObject;
        timer = overlay.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (thisSpellcard.onCooldown)
        {
            if (!overlay.activeSelf)
            {
                overlay.SetActive(true);   
            }
            timer.text = MathF.Round(thisSpellcard.currentCooldownTime).ToString();
        }
        else
        {
            overlay.SetActive(false);
            Destroy(this);
        }
    }
}
