using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowTowerInfo : MonoBehaviour
{
    private Camera cam;
    [Header("[CAMERA]")] 
    public float defaultCameraSize;
    public float focusedCameraSize;
    public float cameraMoveSpeed;

    [Header("[CACHE]")]
    public Tower selectedTower;
    public GameObject towerInfoUI;
    public TextMeshProUGUI towerNameUI;
    public TextMeshProUGUI towerDamageUI;
    public TextMeshProUGUI towerFireRateUI;
    public TextMeshProUGUI towerBlockAmountUI;
    public Transform healthBar;
    public TextMeshProUGUI healthNumber;
    public Button spellCardButtonTemplate;
    public Transform spellcardUI;
    private bool movingCamera;
    private bool returningCamera;
    private bool hasFoundTower;
    public float currentState;
    public bool canShowUI;
    private Tower currentFocusedTower;
    public GameObject fastForwardButton;
    public GameObject pauseButton;
    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !selectedTower)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, LayerMask.GetMask("Tower"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent(out Tower isTower))
                {
                    currentFocusedTower = isTower;
                }
                else if (hit.collider.gameObject.TryGetComponent(out TowerBlockingCollision isMeleeTower))
                {
                    currentFocusedTower = isMeleeTower.thisTower;
                }
                if (currentFocusedTower)
                {
                    ShowTowerInfoUI(currentFocusedTower);
                    currentFocusedTower.GetComponentInChildren<TowerRangeCollider>().showRange.SetActive(true);
                    fastForwardButton.SetActive(false);
                    pauseButton.SetActive(false);
                }
            }
        }
        if (movingCamera)
        {
            currentState += Time.deltaTime * cameraMoveSpeed;
            cam.transform.position = Vector3.Lerp(new Vector3(0,0,-10), new Vector3(selectedTower.transform.position.x, selectedTower.transform.position.y, -10), currentState);
            cam.orthographicSize = Mathf.Lerp(defaultCameraSize, focusedCameraSize, currentState);
            if (currentState >= 1)
            {
                movingCamera = false;   
            }
        }
        if (returningCamera)
        {
            if (selectedTower)
            {
                currentState -= Time.deltaTime * cameraMoveSpeed;
                cam.transform.position = Vector3.Lerp(new Vector3(0,0,-10), new Vector3(selectedTower.transform.position.x, selectedTower.transform.position.y, -10), currentState);
                cam.orthographicSize = Mathf.Lerp(defaultCameraSize, focusedCameraSize, currentState);
                if (currentState <= 0)
                {
                    returningCamera = false;   
                    HideTowerInfoUI();
                }
            }
            else
            {
                cam.transform.position = new Vector3(0, 0, -10);
                HideTowerInfoUI();
                returningCamera = false;
            }
        }
        if (towerInfoUI.activeSelf && currentState > 0.5f && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            returningCamera = true;
            currentFocusedTower.GetComponentInChildren<TowerRangeCollider>().showRange.SetActive(false);
            currentFocusedTower = null;
        }
    }

    public void ShowTowerInfoUI(Tower towerToShow)
    {
        if (canShowUI)
        {
            selectedTower = towerToShow;
            towerInfoUI.SetActive(true);
            movingCamera = true;
            //GameObject showRange = Instantiate(towerToShow.range.showRange, towerToShow.transform);
            //showRange.SetActive(true);
            //showRange.transform.rotation = towerToShow.transform.rotation;
            towerNameUI.text = towerToShow.towerID;
            towerDamageUI.text = towerToShow.stats.damage.ToString();
            towerFireRateUI.text = towerToShow.stats.fireRate + "s";
            towerBlockAmountUI.text = towerToShow.stats.blockAmount.ToString();
            cam.transform.position = new Vector3(towerToShow.transform.position.x, towerToShow.transform.position.y, -10);
            cam.orthographicSize = focusedCameraSize;
            RebuildSpellcardList(); 

        }
    }

    public void RebuildSpellcardList()
    {
        foreach (Transform child in spellcardUI)
        {
            Destroy(child.gameObject);
        }

        foreach (Spellcard spellcard in selectedTower.spellcardList)
        {
            Button newSpellcardButton = Instantiate(spellCardButtonTemplate, spellcardUI);
            newSpellcardButton.onClick.AddListener(() => spellcard.CastSpellCard());
            newSpellcardButton.transform.Find("SpellcardName").GetComponent<TextMeshProUGUI>().text =
                spellcard.spellcardID;
            newSpellcardButton.transform.Find("SpellcardImage").GetComponent<Image>().sprite =
                spellcard.spellcardImage;
            newSpellcardButton.transform.Find("SpellcardText").GetComponent<TextMeshProUGUI>().text =
                spellcard.spellcardDescription;
            newSpellcardButton.gameObject.SetActive(true);
            SpellcardButtonUI newSpellcardButtonUI = newSpellcardButton.AddComponent<SpellcardButtonUI>();
            newSpellcardButtonUI.thisSpellcard = spellcard;
        }
    }
    public void HideTowerInfoUI()
    {
        towerInfoUI.SetActive(false);
        fastForwardButton.SetActive(true);
        pauseButton.SetActive(true);
        selectedTower = null;
    }

    public void UpdateTowerHealthBar()
    {
        healthBar.localScale = new Vector3(selectedTower.health / selectedTower.maxHealth, 1f);
        healthNumber.text = selectedTower.health + "/" + selectedTower.maxHealth;
    }
}
