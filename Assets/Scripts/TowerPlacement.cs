using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    // [caching]
    private Camera cam;
    private Grid placementGrid;
    private Transform towersFolder;
    
    // [placement stuff]
    public List<TowerToPlace> availableTowers;
    public Tower selectedTower;
    public bool isPlacing;
    
    // [placement UI]
    public Transform placementFrame;
    public Button buttonTemplate;
    
    private void Start()
    {
        towersFolder = GameObject.Find("TowersFolder").transform;
        placementGrid = FindFirstObjectByType<Grid>();
        cam = Camera.main;
        foreach (TowerToPlace towerToPlace in availableTowers)
        {
            Button newButton = Instantiate(buttonTemplate, placementFrame);
            newButton.onClick.AddListener(() => SetPlacement(towerToPlace));
            newButton.GetComponentInChildren<Text>().text = towerToPlace.tower.towerID;
        }
    }

    private void Update()
    {
        if (isPlacing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,cam.nearClipPlane + 10));
                Tower newTower = Instantiate(selectedTower, towersFolder);
                newTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
                selectedTower.isPlaced = true;
            }
        }
    }

    private void SetPlacement(TowerToPlace towerChosen)
    {
        if (!towerChosen.onCooldown)
        {
            return;
        }
        selectedTower = towerChosen.tower;
        isPlacing = true;
    }
}
