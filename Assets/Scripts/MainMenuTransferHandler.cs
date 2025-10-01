using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTransferHandler : MonoBehaviour
{
    public static MainMenuTransferHandler instance;
    public class SelectedTowerData
    {
        public string towerID;
        public int targettingMode;
    }
    public HashSet<SelectedTowerData> selectedTowers = new();
    public string mapIDSelected;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
