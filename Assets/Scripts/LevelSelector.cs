using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [System.Serializable]
    public class LevelSelection
    {
        public string sceneName;
        public string levelName;
        public bool unlocked;
    }
    public List<LevelSelection> levels;
    public MainMenuTowerSelectionUIHandler towerSelector;
    public Transform levelUI;
    public GameObject towerSelectionUI;
    public GameObject levelSelectionUI;
    public Button templateLevelButton;
    private void Start()
    {
        foreach (LevelSelection level in levels)
        {
            if (level.unlocked)
            {
                Button newButton = Instantiate(templateLevelButton, levelUI);
                newButton.onClick.AddListener(() => SelectLevel(level.sceneName));
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = level.levelName;
                newButton.gameObject.SetActive(true);
            }
        }
    }

    public void SelectLevel(string levelToSelect)
    {
        towerSelector.levelSelected = levelToSelect;
        levelSelectionUI.SetActive(false);
        towerSelectionUI.SetActive(true);
    }
}
