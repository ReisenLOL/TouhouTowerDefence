using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTowerInfo : MonoBehaviour
{
    private Camera cam;
    [Header("[CAMERA]")] 
    public float defaultCameraSize;
    public float focusedCameraSize;
    public float cameraMoveSpeed;

    [Header("[CACHE]")]
    private Tower selectedTower;
    public GameObject towerInfoUI;
    public TextMeshProUGUI towerNameUI;
    public TextMeshProUGUI towerDamageUI;
    public TextMeshProUGUI towerFireRateUI;
    public TextMeshProUGUI towerBlockAmountUI;
    private bool movingCamera;
    private bool returningCamera;
    public float currentState;
    public bool canShowUI;
    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, LayerMask.GetMask("Tower"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent(out Tower isTower))
                {
                    ShowTowerInfoUI(isTower);
                }
                else if (hit.collider.gameObject.TryGetComponent(out TowerBlockingCollision isMeleeTower))
                {
                    ShowTowerInfoUI(isMeleeTower.thisTower);
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
            currentState -= Time.deltaTime * cameraMoveSpeed;
            cam.transform.position = Vector3.Lerp(new Vector3(0,0,-10), new Vector3(selectedTower.transform.position.x, selectedTower.transform.position.y, -10), currentState);
            cam.orthographicSize = Mathf.Lerp(defaultCameraSize, focusedCameraSize, currentState);
            if (currentState <= 0)
            {
                returningCamera = false;   
                HideTowerInfoUI();
            }
        }

        if (towerInfoUI.activeSelf && currentState > 0.5f && Input.GetMouseButtonDown(0))
        {
            returningCamera = true;
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
            towerDamageUI.text = "Damage: " + towerToShow.stats.damage;
            towerFireRateUI.text = "FireRate: " + towerToShow.stats.fireRate + "s";
            towerBlockAmountUI.text = "Block Amount: " + towerToShow.stats.blockAmount;
            cam.transform.position = new Vector3(towerToShow.transform.position.x, towerToShow.transform.position.y, -10);
            cam.orthographicSize = focusedCameraSize;   
        }
    }

    public void HideTowerInfoUI()
    {
        towerInfoUI.SetActive(false);
        selectedTower = null;
    }
}
