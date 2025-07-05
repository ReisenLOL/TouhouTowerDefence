using System;
using TMPro;
using UnityEngine;

public class TowerButtonUI : MonoBehaviour
{
    private GameObject overlay;
    private TextMeshProUGUI timer;
    public TowerToPlace thisTower;
    public bool showTimer;

    private void Start()
    {
        overlay = transform.Find("RespawnPanel").gameObject;
        timer = overlay.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (thisTower.onCooldown)
        {
            if (!overlay.activeSelf)
            {
                overlay.SetActive(true);   
            }
            timer.text = MathF.Round(thisTower.currentCooldownTimer).ToString();
        }
        else
        {
            overlay.SetActive(false);
            Destroy(this);
        }
    }
}
