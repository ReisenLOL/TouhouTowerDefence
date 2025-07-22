using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTowersTransferHandler : MonoBehaviour
{
    public static SelectedTowersTransferHandler instance;
    public class SelectedTowerData
    {
        public string towerID;
        public int targettingMode;
    }
    public HashSet<SelectedTowerData> selectedTowers = new();
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
