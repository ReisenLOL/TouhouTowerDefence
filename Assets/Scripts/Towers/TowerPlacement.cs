using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    // [caching]
    private Camera cam;
    private Grid placementGrid;
    private Transform towersFolder;
    private Transform towerToRotate;
    private Transform rangeToRotate; 
    //THIS IS SO FUCKING STUPID BUT IT'S ALL I CAN THINK OF AND IT WORKS.
    [SerializeField] private float timeUntilRotation;
    private float currentTimeUntilRotation;
    private bool waitForRotation;
    private ShowTowerInfo showTowerInfo;
    
    [Header("[PLACEMENT STUFF]")]
    public List<TowerToPlace> towersToAdd; //please don't use this ever it's for the prefabs if you use it you edit the prefabs. who are you talking to sylvia? no one else is gonna be developing this game. i think.
    public List<TowerToPlace> availableTowers;
    public TowerToPlace selectedTower;
    private bool isPlacing;
    private bool selectingPosition;
    private bool selectingRotation;
    private GameObject placeholderTower;
    private GameObject placeholderRange;
    private bool createPlaceholder;
    
    [Header("[PLACEMENT UI]")]
    public Transform placementFrame;
    public Button buttonTemplate;

    [Header("Resources")] 
    public int currentPower;
    public float powerGenerationRate;
    private float currentPowerGenerationTime;
    public TextMeshProUGUI powerNumberUI;
    
    private void Start()
    {
        towersFolder = GameObject.Find("TowersFolder").transform;
        placementGrid = FindFirstObjectByType<Grid>();
        showTowerInfo = FindFirstObjectByType<ShowTowerInfo>();
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
        currentPowerGenerationTime += Time.deltaTime;
        if (currentPowerGenerationTime >= powerGenerationRate)
        {
            currentPowerGenerationTime -= powerGenerationRate;
            currentPower++;
            powerNumberUI.text = "P " + currentPower;
        }
        if (isPlacing)
        {
            if (createPlaceholder)
            {
                placeholderTower = Instantiate(selectedTower.tower.gameObject);
                placeholderTower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
                placeholderTower.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                if (placeholderTower.GetComponentInChildren<TowerBlockingCollision>())
                {
                    Destroy(placeholderTower.GetComponentInChildren<TowerBlockingCollision>().gameObject);   
                }
                foreach (MonoBehaviour script in placeholderTower.GetComponents<MonoBehaviour>())
                {
                    Destroy(script);
                }
                createPlaceholder = false;
            }
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,cam.nearClipPlane + 10));
            if (placeholderTower)
            {
                placeholderTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
            }
            if (selectingPosition)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, LayerMask.GetMask("Terrain")); //i don't remember layermasks being that way.
                    if (selectedTower.tower.isCliffTower && hit.collider.gameObject.CompareTag("Path") || !selectedTower.tower.isCliffTower && hit.collider.gameObject.CompareTag("Cliff") || !hit)
                    {
                        return;
                    }
                    Tower newTower = Instantiate(selectedTower.tower, towersFolder);
                    placeholderRange = Instantiate(newTower.range.showRange.gameObject, placeholderTower.transform);
                    placeholderRange.SetActive(true);
                    foreach (MonoBehaviour script in placeholderRange.GetComponents<MonoBehaviour>())
                    {
                        Destroy(script);
                    }
                    placeholderRange.transform.SetParent(newTower.transform);
                    placeholderRange.transform.position = newTower.transform.position;
                    Destroy(placeholderTower);
                    newTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
                    selectedTower.isPlaced = true;
                    TowerRangeCollider addRange = Instantiate(selectedTower.tower.range, newTower.transform); ;
                    addRange.thisTower = newTower;
                    rangeToRotate = addRange.transform; //also add rotation of towers ONCE YOU CAN DRAW.
                    RebuildTowerSelection();
                    currentPower -= selectedTower.powerCost;
                    selectingPosition = false;
                    waitForRotation = true;
                }
            }

            if (waitForRotation)
            {
                currentTimeUntilRotation += Time.deltaTime;
                if (currentTimeUntilRotation >= timeUntilRotation)
                {
                    selectingRotation = true;
                    currentTimeUntilRotation = 0;
                }
            }
            if (selectingRotation)
            {
                Time.timeScale = 0.1f;
                if (Input.GetMouseButton(0))
                {
                    Vector2 direction = worldPos - rangeToRotate.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    placeholderRange.transform.rotation = Quaternion.Euler(0, 0, angle);
                    placeholderRange.transform.rotation = Quaternion.Euler(SnapToCardinalDirection(rangeToRotate));
                    rangeToRotate.rotation = Quaternion.Euler(0, 0, angle);
                    rangeToRotate.rotation = Quaternion.Euler(SnapToCardinalDirection(rangeToRotate));
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Time.timeScale = 1;
                    Destroy(placeholderRange);
                    selectingRotation = false;
                    isPlacing = false;
                    showTowerInfo.canShowUI = true;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 1;
                Destroy(placeholderTower);
                selectingPosition = false;
                selectingRotation = false;
                isPlacing = false;
                showTowerInfo.canShowUI = true;
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
                newButton.transform.Find("CostPanel").GetComponentInChildren<TextMeshProUGUI>().text = "P " + towerToPlace.powerCost;
                newButton.GetComponent<TowerButtonUI>().thisTower = towerToPlace;
                newButton.onClick.AddListener(() => SetPlacement(towerToPlace));   
            }
        }
    }
    public void SetPlacement(TowerToPlace towerChosen) //YOU'RE A FUCKING DU- no i should be more kind. c:
    {
        if (towerChosen.onCooldown || towerChosen.isPlaced || currentPower < towerChosen.powerCost) //i stand corrected, you really are a fucking dumbass, sylvia.
        {
            return;
        }
        selectedTower = towerChosen;
        selectingPosition = true;
        createPlaceholder = true;
        isPlacing = true;
        showTowerInfo.canShowUI = false;
    }

    private Vector3 SnapToCardinalDirection(Transform tower)
    {
        float currentAngle = tower.eulerAngles.z;
        float snappedAngle = MathF.Round(currentAngle/90f) * 90f;
        return new Vector3(0, 0, snappedAngle);
    }
}
