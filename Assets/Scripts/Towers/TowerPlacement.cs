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
    public List<TowerToPlace> towersToAdd; //please don't use this ever it's for the prefabs if you use it you edit the prefabs. who are you talking to sylvia? no one else is gonna be developing this game. i think.
    public List<TowerToPlace> availableTowers;
    public TowerToPlace selectedTower;
    private bool isPlacing;
    private bool selectingPosition;
    private bool selectingRotation;
    private Vector3 rotationAmount;
    private GameObject placeholderTower;
    private GameObject placeholderRange;
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
                rotationAmount = Vector3.zero;
                placeholderTower = Instantiate(selectedTower.tower.gameObject);
                placeholderTower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
                placeholderRange = Instantiate(placeholderTower.GetComponent<Tower>().range.showRange.gameObject, placeholderTower.transform);
                placeholderRange.SetActive(true);
                foreach (MonoBehaviour script in placeholderRange.GetComponents<MonoBehaviour>())
                {
                    Destroy(script);
                }
                foreach (MonoBehaviour script in placeholderTower.GetComponents<MonoBehaviour>())
                {
                    Destroy(script);
                }
                createPlaceholder = false;
            }
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,cam.nearClipPlane + 10));
            placeholderTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
            if (Input.GetKeyDown(KeyCode.R))
            {
                // i'll make the mouse drag way later.
                if (rotationAmount.z >= 360)
                {
                    rotationAmount.z = 0;
                }
                rotationAmount.z += 90;
                placeholderRange.transform.rotation = Quaternion.Euler(rotationAmount);
            }
            if (selectingPosition)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //add a check for cliff towers later.
                    Destroy(placeholderTower);
                    Tower newTower = Instantiate(selectedTower.tower, towersFolder);
                    newTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
                    selectedTower.isPlaced = true;
                    TowerRangeCollider addRange = Instantiate(selectedTower.tower.range, newTower.transform);
                    addRange.transform.rotation = Quaternion.Euler(rotationAmount);
                    addRange.thisTower = newTower;
                    RebuildTowerSelection();
                    isPlacing = false;
                }
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
        selectingPosition = true;
        createPlaceholder = true;
        isPlacing = true;
    }
}
