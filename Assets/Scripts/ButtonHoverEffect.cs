using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float speedToMove;
    public Vector2 offset;
    private RectTransform rectTransform;
    public Vector2 targetPosition;
    private bool checkHovering;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (checkHovering)
        {
            if (rectTransform.anchoredPosition != targetPosition)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, speedToMove * Time.deltaTime);
            }
            else
            {
                checkHovering = false;
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = Vector2.zero + offset;
        checkHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = Vector2.zero;
        checkHovering = true;
    }
}