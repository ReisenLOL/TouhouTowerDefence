using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    public Effect effectToApply;
    public float effectLenth;
    public float currentEffectTime;
    public Unit affectedUnit;
    public GameObject effectIconSquare;
    public List<GameObject> effectVisuals = new();
    public Transform durationBar;
    private void Update()
    {
        currentEffectTime += Time.deltaTime;
        durationBar.localScale = new Vector3((effectLenth - currentEffectTime)/effectLenth, 1f, 1f);
        if (currentEffectTime >= effectLenth)
        {
            effectToApply.RemoveEffect(this);
        }
    }
}
