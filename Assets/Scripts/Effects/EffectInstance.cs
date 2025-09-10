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
    private void Update()
    {
        currentEffectTime += Time.deltaTime;
        if (currentEffectTime >= effectLenth)
        {
            effectToApply.RemoveEffect(this);
        }
    }
}
