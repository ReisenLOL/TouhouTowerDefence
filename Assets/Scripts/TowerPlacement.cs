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
    public List<TowerToPlace> towersToAdd; //please don't use this ever it's for the prefabs if you use it you edit the prefabs.
    public List<TowerToPlace> availableTowers;
    public TowerToPlace selectedTower;
    private bool isPlacing;
    private GameObject placeholderTower;
    private bool createPlaceholder;
    
    // [placement UI]
    public Transform placementFrame;
    public Button buttonTemplate;
    
    private void Start()
    {
        towersFolder = GameObject.Find("TowersFolder").transform;
        placementGrid = FindFirstObjectByType<Grid>();
        cam = Camera.main;
        foreach (TowerToPlace towerToPlace in towersToAdd)
        {
            TowerToPlace newTower = Instantiate(towerToPlace, transform);
            availableTowers.Add(newTower);
        }
        RebuildTowerSelection();
    }

    private void Update()
    {
        if (isPlacing)
        {
            if (createPlaceholder)
            {
                placeholderTower = Instantiate(selectedTower.tower.gameObject);
                placeholderTower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
                foreach (MonoBehaviour script in placeholderTower.GetComponents<MonoBehaviour>())
                {
                    Destroy(script);
                }
                createPlaceholder = false;
            }
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,cam.nearClipPlane + 10));
            placeholderTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(placeholderTower);
                Tower newTower = Instantiate(selectedTower.tower, towersFolder);
                newTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
                selectedTower.isPlaced = true;
                isPlacing = false;
                RebuildTowerSelection();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(placeholderTower);
                isPlacing = false;
            }
        }
    }

    public void RebuildTowerSelection()
    {
        foreach (Transform child in placementFrame)
        {
            Destroy(child.gameObject);
        }
        foreach (TowerToPlace towerToPlace in availableTowers)
        {
            if (!towerToPlace.isPlaced)
            {
                Button newButton = Instantiate(buttonTemplate, placementFrame);
                newButton.gameObject.SetActive(true);
                newButton.transform.Find("TowerImage").GetComponent<Image>().sprite = towerToPlace.portrait;
                newButton.GetComponent<TowerButtonUI>().thisTower = towerToPlace;
                newButton.onClick.AddListener(() => SetPlacement(towerToPlace));   
            }
        }
    }
    public void SetPlacement(TowerToPlace towerChosen) //YOU'RE A FUCKING DU- no i should be more kind. c:
    {
        if (towerChosen.onCooldown || towerChosen.isPlaced) //i stand corrected, you really are a fucking dumbass, sylvia.
        {
            return;
        }
        selectedTower = towerChosen;
        createPlaceholder = true;
        isPlacing = true;
    }
}
