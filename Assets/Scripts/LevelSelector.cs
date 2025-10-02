using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [System.Serializable]
    public class LevelSelection
    {
        public string mapID;
        public string levelName;
        public bool unlocked;
    }
    public List<LevelSelection> levels;
    public Transform levelUI;
    public GameObject towerSelectionUI;
    public GameObject levelSelectionUI;
    public Button templateLevelButton;
    private string savePath => Path.Combine(Application.persistentDataPath, "MapsSaved.json");
    private void Start()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameManager.MapSaveData savedata = JsonUtility.FromJson<GameManager.MapSaveData>(json);
            foreach (LevelSelection level in levels)
            {
                if (savedata.unlockedMaps.Contains(level.mapID))
                {
                    level.unlocked = true;
                }
            }
        }
        foreach (LevelSelection level in levels)
        {
            if (level.unlocked)
            {
                Button newButton = Instantiate(templateLevelButton, levelUI);
                newButton.onClick.AddListener(() => SelectLevel(level.mapID));
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = level.levelName;
                newButton.gameObject.SetActive(true);
            }
        }
    }
    public void SelectLevel(string levelToSelect)
    {
        MainMenuTransferHandler.instance.mapIDSelected = levelToSelect;
        levelSelectionUI.SetActive(false);
        towerSelectionUI.SetActive(true);
    }
}
