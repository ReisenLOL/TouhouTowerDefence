using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float defence = 1f;
    public float attackModifier = 1f;
    protected SpriteRenderer[] spriteRenderers;
    private float currentState;
    public float damageColorChangeSpeed = 4f;

    protected virtual void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage * defence;
        if (health <= 0)
        {
            OnKill();
        }
        else
        {
            currentState = 0;
            StartCoroutine(DamageAnimation());
        }
    }

    protected virtual void OnKill()
    {
        
    }
    private IEnumerator DamageAnimation()
    {
        while (currentState < 1)
        {
            currentState += Time.deltaTime * damageColorChangeSpeed;
            foreach (SpriteRenderer spritepart in spriteRenderers)
            {
                if (spritepart)
                {
                    spritepart.color = Color.Lerp(Color.red, Color.white, currentState);
                }
            }
            yield return null;
        }
    }
}
