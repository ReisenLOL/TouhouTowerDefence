using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTowerSelectionUIHandler : MonoBehaviour
{
    public List<TowerToPlace> selectableTowers = new();
    [Header("[CACHE]")]
    public Button templateButton;
    public RectTransform towerListUI;
    public TowerToPlace selectedTower;
    public GameObject showSelectedPanel;
    public Animator selectionAnimator;
    [Header("[TOWER SELECTION UI]")]
    public RectTransform towerSelectionUI;
    public Image towerSprite;
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerFireRate;
    public TextMeshProUGUI towerBlockAmount;
    public TMP_Dropdown towerModeSelection;
    public Button selectTowerButton;
    public string levelSelected;
    private void Start()
    {
        foreach (TowerToPlace towerToPlace in selectableTowers)
        {
            Button newButton = Instantiate(templateButton, towerListUI);
            newButton.transform.Find("TowerImage").GetComponent<Image>().sprite = towerToPlace.portrait;
            newButton.gameObject.SetActive(true);
            newButton.onClick.AddListener(() => SelectTower(towerToPlace, newButton.transform));
        }
        foreach (string towerMode in Enum.GetNames(typeof(Tower.TargettingModes)))
        {
            TMP_Dropdown.OptionData newDropDown = new TMP_Dropdown.OptionData
            {
                text = towerMode
            };
            towerModeSelection.options.Add(newDropDown);   
        }
    }

    public void SelectTower(TowerToPlace towerSelected, Transform buttonSelected)
    {
        if (selectedTower == towerSelected)
        {
            //selectionAnimator.SetTrigger("Deselect");
        }
        else
        {
            selectedTower = towerSelected;
            selectionAnimator.SetTrigger("Select");
            ShowSelectionInfo();
        } 
        showSelectedPanel = buttonSelected.Find("SelectionPanel").gameObject;
    }

    public void ShowSelectionInfo()
    {
        towerSelectionUI.gameObject.SetActive(true);
        towerName.text = selectedTower.tower.towerID;
        towerDamage.text = "Damage: " + selectedTower.tower.stats.damage;
        towerFireRate.text = "Fire Rate: " + selectedTower.tower.stats.fireRate;
        towerBlockAmount.text = "Block Amount: " + selectedTower.tower.stats.blockAmount;
        towerSprite.sprite = selectedTower.portrait;
        towerModeSelection.value = 0;
        foreach (MainMenuTransferHandler.SelectedTowerData towerData in MainMenuTransferHandler.instance.selectedTowers)
        {
            if (towerData.towerID == selectedTower.tower.towerID)
            {            
                towerModeSelection.value = towerData.targettingMode;
                break;
            }
        }
        selectTowerButton.onClick.RemoveAllListeners();
        selectTowerButton.onClick.AddListener(() => AddTower());
    }

    public void AddTower()
    {
        showSelectedPanel.SetActive(true);
        bool notFoundInList = false;
        foreach (MainMenuTransferHandler.SelectedTowerData towerData in MainMenuTransferHandler.instance.selectedTowers)
        {
            if (towerData.towerID == selectedTower.tower.towerID)
            {
                MainMenuTransferHandler.instance.selectedTowers.Remove(towerData);
                notFoundInList = true;
                showSelectedPanel.SetActive(false);
                break;
            }
        }
        if (!notFoundInList)
        {
            MainMenuTransferHandler.SelectedTowerData newTowerData =
                new MainMenuTransferHandler.SelectedTowerData();
            newTowerData.towerID = selectedTower.tower.towerID;
            newTowerData.targettingMode = towerModeSelection.value;
            MainMenuTransferHandler.instance.selectedTowers.Add(newTowerData);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
