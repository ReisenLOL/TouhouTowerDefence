using System;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    public Effect effectToApply;
    public float effectLenth;
    public float currentEffectTime;
    public Unit affectedUnit;
    public GameObject effectIconSquare;
    private void Update()
    {
        currentEffectTime += Time.deltaTime;
        if (currentEffectTime >= effectLenth)
        {
            effectToApply.RemoveEffect(this);
        }
    }
}
