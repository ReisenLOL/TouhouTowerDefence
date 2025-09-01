using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [Header("[CACHE]")] 
    [SerializeField] private GameObject dragHandle;
    [SerializeField] private LineRenderer dragLine;
    private GameObject newDragHandle;
    private LineRenderer newDragLine;
    private Camera cam;
    private Grid placementGrid;
    private Transform towersFolder;
    private Transform towerToRotate;
    private Transform rangeToRotate;
    private Vector3 worldPos;
    private Transform towerSprite;
    private Transform createdSelectionSquare;
    //THIS IS SO FUCKING STUPID BUT IT'S ALL I CAN THINK OF AND IT WORKS.
    [SerializeField] private float timeUntilRotation;
    private float currentTimeUntilRotation;
    private bool waitForRotation;
    private ShowTowerInfo showTowerInfo;
    private float currentState;
    public float placementSquareFadeSpeed;
    private SpriteRenderer placementSquareSprite;
    private bool reverseFade;

    [Header("[PLACEMENT STUFF]")] 
    public Transform towerSelectionSquare;
    public AudioClip placementSound;
    private AudioSource audioSource;
    public List<TowerToPlace> AllTowers; //please don't use this ever it's for the prefabs if you use it you edit the prefabs. who are you talking to sylvia? no one else is gonna be developing this game. i think.
    public List<TowerToPlace> availableTowers;
    public TowerToPlace selectedTower;
    private bool isPlacing;
    private bool selectingPosition;
    private bool selectingRotation;
    private GameObject placeholderTower;
    private GameObject placeholderRange;
    public Tower newlyCreatedTower;
    
    [Header("[PLACEMENT UI]")]
    public Transform placementFrame;
    public Button buttonTemplate;

    [Header("[RESOURCES]")] 
    public int currentPower;
    public float powerGenerationRate;
    private float currentPowerGenerationTime;
    public TextMeshProUGUI powerNumberUI;
    //time to do a bunch more visual stuffs.
    [Header("[VISUALS]")] 
    public Tilemap cliffMap;
    public Tilemap pathMap;
    public Tilemap unplaceableMap;
    public Color noPlace;
    public Color yesPlace;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        towersFolder = GameObject.Find("TowersFolder").transform;
        placementGrid = FindFirstObjectByType<Grid>();
        cliffMap = GameObject.Find("Cliff Map").GetComponent<Tilemap>();
        pathMap = GameObject.Find("Path Map").GetComponent<Tilemap>();
        unplaceableMap = GameObject.Find("Unplaceable Map").GetComponent<Tilemap>();
        showTowerInfo = FindFirstObjectByType<ShowTowerInfo>();
        cam = Camera.main;
        if (SelectedTowersTransferHandler.instance)
        {
            foreach (SelectedTowersTransferHandler.SelectedTowerData selectedTowers in SelectedTowersTransferHandler.instance.selectedTowers)
            {
                foreach (TowerToPlace towerToPlace in AllTowers)
                {
                    if (selectedTowers.towerID == towerToPlace.tower.towerID)
                    {
                        TowerToPlace newTower = Instantiate(towerToPlace, transform);
                        newTower.tower.currentTargettingMode = (Tower.TargettingModes)selectedTowers.targettingMode;
                        availableTowers.Add(newTower);
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (TowerToPlace towerToPlace in AllTowers)
            {
                TowerToPlace newTower = Instantiate(towerToPlace, transform);
                availableTowers.Add(newTower);
            }
        }
        RebuildTowerSelection();
    }

    private void Update()
    {
        HandlePowerGeneration(); // is this what they call "code optimization"? no it isn't lol you just split the update function into more functions, but that's more readable so that's fine
        HandlePlacement();
    }

    private void HandlePlacement()
    {
        if (!isPlacing)
        {
            return;
        }
        if (isPlacing)
        {
            worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
            if (placeholderTower)
            {
                Vector3 setPosition = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
                placeholderTower.transform.position = setPosition;
                createdSelectionSquare.position = setPosition;
            }
            else if (!selectingRotation)
            {
                CreatePlaceholderTower();
            }
            if (selectingPosition)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            if (waitForRotation)
            {
                RotationDelay();
            }
            if (selectingRotation)
            {
                RotateTower();
                if (Input.GetMouseButtonUp(0))
                {
                    if (newlyCreatedTower.animator)
                    {
                        newlyCreatedTower.animator.Play(newlyCreatedTower.deployAnimParam);
                    }
                    StopPlacement();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                StopPlacement();
            }
        }
    }

    private void RotateTower()
    {

        Time.timeScale = 0.1f;
        if (Input.GetMouseButton(0))
        {
            newDragHandle.transform.position = worldPos;
            newDragLine.SetPosition(0, placeholderRange.transform.position);
            newDragLine.SetPosition(1, newDragHandle.transform.position);
            Vector2 direction = worldPos - rangeToRotate.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            placeholderRange.transform.rotation = Quaternion.Euler(0, 0, angle);
            placeholderRange.transform.rotation = Quaternion.Euler(SnapToCardinalDirection(rangeToRotate));
            rangeToRotate.rotation = Quaternion.Euler(0, 0, angle);
            rangeToRotate.rotation = Quaternion.Euler(SnapToCardinalDirection(rangeToRotate));
            if (rangeToRotate.rotation.z == 0 || rangeToRotate.rotation.z == 360)
            {
                towerSprite.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                towerSprite.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
    private void PlaceTower()
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, LayerMask.GetMask("Terrain")); //i don't remember layermasks being that way.
        if (selectedTower.tower.stats.isCliffTower && hit.collider.gameObject.CompareTag("Path") || !selectedTower.tower.stats.isCliffTower && hit.collider.gameObject.CompareTag("Cliff") || !hit)
        {
            return;
        }
        audioSource.PlayOneShot(placementSound);
        newlyCreatedTower = Instantiate(selectedTower.tower, towersFolder);
        if (selectedTower.tower.currentTargettingMode == Tower.TargettingModes.Focused)
        {
            placeholderRange = Instantiate(newlyCreatedTower.focusedRange.showRange, newlyCreatedTower.transform);
        }
        else
        {
            placeholderRange = Instantiate(newlyCreatedTower.scatteredRange.showRange, newlyCreatedTower.transform);
        }
        placeholderRange.SetActive(true);
        foreach (MonoBehaviour script in placeholderRange.GetComponents<MonoBehaviour>())
        {
            Destroy(script);
        }
        placeholderRange.transform.SetParent(newlyCreatedTower.transform);
        placeholderRange.transform.position = newlyCreatedTower.transform.position;
        newlyCreatedTower.transform.position = placementGrid.GetCellCenterWorld(placementGrid.WorldToCell(worldPos));
        selectedTower.isPlaced = true;
        cliffMap.color = Color.white;
        pathMap.color = Color.white;
        unplaceableMap.color = Color.white;
        TowerRangeCollider addRange;
        if (selectedTower.tower.currentTargettingMode == Tower.TargettingModes.Focused)
        {
            addRange = Instantiate(newlyCreatedTower.focusedRange, newlyCreatedTower.transform);
        }
        else
        {
            addRange = Instantiate(newlyCreatedTower.scatteredRange, newlyCreatedTower.transform);
        }
        addRange.thisTower = newlyCreatedTower;
        rangeToRotate = addRange.transform; //also add rotation of towers ONCE YOU CAN DRAW.
        RebuildTowerSelection();
        towerSprite = newlyCreatedTower.transform.Find("Sprite").transform;
        currentPower -= selectedTower.powerCost;
        selectingPosition = false;
        waitForRotation = true;
        newDragHandle = Instantiate(dragHandle);
        newDragHandle.transform.position = placeholderRange.transform.position;
        newDragLine = Instantiate(dragLine);
    }

    private void StopPlacement()
    {
        Time.timeScale = 1;
        if (placeholderRange)
        {
            Destroy(placeholderRange);
        }
        cliffMap.color = Color.white;
        pathMap.color = Color.white;
        unplaceableMap.color = Color.white;
        if (placeholderTower)
        {
            Destroy(placeholderTower);
            Destroy(createdSelectionSquare.gameObject);
        }
        if (newDragHandle)
        {
            Destroy(newDragLine.gameObject);
            Destroy(newDragHandle);   
        }
        placementFrame.gameObject.SetActive(true);
        selectingPosition = false;
        selectingRotation = false;
        isPlacing = false;
        showTowerInfo.canShowUI = true;
    }
    private void RotationDelay()
    {
        currentTimeUntilRotation += Time.deltaTime;
        if (currentTimeUntilRotation >= timeUntilRotation)
        {
            selectingRotation = true;
            Destroy(placeholderTower);
            Destroy(createdSelectionSquare.gameObject);
            waitForRotation = false;
            currentTimeUntilRotation = 0;
        }
    }
    private void HandlePowerGeneration()
    {
        currentPowerGenerationTime += Time.deltaTime;
        if (currentPowerGenerationTime >= powerGenerationRate)
        {
            currentPowerGenerationTime -= powerGenerationRate;
            currentPower++;
            powerNumberUI.text = "P " + currentPower;
        }
    }
    private void CreatePlaceholderTower()
    {
        placeholderTower = Instantiate(selectedTower.tower.gameObject);
        placeholderTower.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        if (placeholderTower.GetComponentInChildren<TowerBlockingCollision>())
        {
            Destroy(placeholderTower.GetComponentInChildren<TowerBlockingCollision>().gameObject);   
        }
        foreach (MonoBehaviour script in placeholderTower.GetComponents<MonoBehaviour>())
        {
            Destroy(script);
        }
        if (placeholderTower.GetComponentInChildren<Animator>())
        {
            Destroy(placeholderTower.GetComponentInChildren<Animator>());
        }
        createdSelectionSquare = Instantiate(towerSelectionSquare);
        createdSelectionSquare.position = placeholderTower.transform.position;
        placementSquareSprite = createdSelectionSquare.GetComponent<SpriteRenderer>();
        StartCoroutine(PlacementSquareAnimation());
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
        if (isPlacing || towerChosen.onCooldown || towerChosen.isPlaced || currentPower < towerChosen.powerCost) //i stand corrected, you really are a fucking dumbass, sylvia.
        {
            return;
        }
        if (towerChosen.tower.stats.isCliffTower)
        {
            cliffMap.color = yesPlace;
            pathMap.color = noPlace;
        }
        else
        {
            cliffMap.color = noPlace;
            pathMap.color = yesPlace;
        }

        unplaceableMap.color = noPlace;
        selectedTower = towerChosen;
        selectingPosition = true;
        isPlacing = true;
        placementFrame.gameObject.SetActive(false);
        showTowerInfo.canShowUI = false;
    }

    private Vector3 SnapToCardinalDirection(Transform tower)
    {
        float currentAngle = tower.eulerAngles.z;
        float snappedAngle = MathF.Round(currentAngle/90f) * 90f;
        return new Vector3(0, 0, snappedAngle);
    }
    private IEnumerator PlacementSquareAnimation()
    {
        if (!placementSquareSprite)
        {
            yield break;
        }
        if (!reverseFade && currentState < 1)
        {
            while (currentState < 1)
            {
                currentState += Time.deltaTime * placementSquareFadeSpeed;
                if (currentState >= 1)
                {
                    reverseFade = false;
                    yield return null;
                }
                placementSquareSprite.color = Color.Lerp(new Color(1,1,1, 0), Color.white, currentState);
                yield return null;
            }
        }
        else if (reverseFade && currentState > 0)
        {
            while (currentState > 0)
            {
                currentState -= Time.deltaTime * placementSquareFadeSpeed;
                if (currentState <= 0)
                {
                    reverseFade = false;
                    yield return null;
                }
                placementSquareSprite.color = Color.Lerp(new Color(1,1,1, 0), Color.white, currentState);
                yield return null;
            }
        }
    }
}
