using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject overlay;
    private TextMeshProUGUI timer;
    public TowerToPlace thisTower;
    public ShowTowerInfo towerInfo;
    public bool showTimer;

    private void Start()
    {
        overlay = transform.Find("RespawnPanel").gameObject;
        timer = overlay.GetComponentInChildren<TextMeshProUGUI>();
        towerInfo = FindFirstObjectByType<ShowTowerInfo>();
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
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!towerInfo.selectedTower)
        {
            towerInfo.ShowTowerPlacementInfo(thisTower.tower);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!towerInfo.selectedTower)
        {
            towerInfo.HideTowerInfoUI();
        }

    }
}
