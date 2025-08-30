using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Effect : ScriptableObject
{
    
    public string effectName;
    public Sprite effectIcon;
    public enum EffectType {Buff, Debuff, Neutral}
    public EffectType thisEffectType;
    public Transform templateEffectSquare;
    public float effectLength;
    public virtual void ApplyEffects(Unit affectedUnit)
    { //i don't like using transform.find, is there any better option??
        EffectInstance newEffectInstance = affectedUnit.AddComponent<EffectInstance>();
        newEffectInstance.effectToApply = this;
        newEffectInstance.effectLenth = effectLength;
        Transform newEffectSquare = Instantiate(templateEffectSquare, affectedUnit.transform.Find("EffectsCanvas"));
        newEffectInstance.effectIconSquare = newEffectSquare.gameObject;
        newEffectSquare.transform.Find("EffectIcon").GetComponent<Image>().sprite = effectIcon;
        if (thisEffectType == EffectType.Debuff)
        {
            newEffectSquare.transform.Find("StatusIcon").eulerAngles = new Vector3(0,0,180);
        }
        else if (thisEffectType == EffectType.Neutral)
        {
            newEffectSquare.transform.Find("StatusIcon").gameObject.SetActive(false);
        }
    }

    public virtual void RemoveEffect(EffectInstance effectInstanceToRemove)
    {
        Destroy(effectInstanceToRemove.effectIconSquare);
    }
}
