using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float defence = 1f;
    public float attackModifier = 1f;
    public bool isDying;
    public bool canMove = true; //this shouldn't be here for towers but whatever!
    protected SpriteRenderer[] spriteRenderers;
    private float currentState;
    public Transform effectOverlayUI;
    [SerializeField] private float damageColorChangeSpeed = 4f;
    [SerializeField] private float deathColorChangeSpeed = 4f;
    private Coroutine deathCoroutine;

    protected virtual void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage * defence;
        if (health <= 0)
        {
            currentState = 0;
            isDying = true;
            OnKill();
            deathCoroutine = StartCoroutine(DeathAnimation());
        }
        else
        {
            currentState = 0;
            StartCoroutine(DamageAnimation());
        }
    }
    public virtual void HealDamage(float healing)
    {
        health += healing;
        StartCoroutine(HealAnimation());
    }

    protected virtual void OnKill()
    {
        
    }
    private IEnumerator DamageAnimation()
    {
        if (!isDying)
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
    private IEnumerator DeathAnimation()
    {
        while (currentState < 1)
        {
            //Debug.Log("STATE: DYING");
            currentState += Time.deltaTime * deathColorChangeSpeed;
            if (currentState >= 1)
            {
                Destroy(gameObject);
            }
            foreach (SpriteRenderer spritepart in spriteRenderers)
            {
                if (spritepart)
                {
                    spritepart.color = Color.Lerp(Color.white, Color.black, currentState);
                }
            }
            yield return null;
        }
    }
    private IEnumerator HealAnimation()
    {
        while (currentState < 1)
        {
            currentState += Time.deltaTime * damageColorChangeSpeed;
            foreach (SpriteRenderer spritepart in spriteRenderers)
            {
                if (spritepart)
                {
                    spritepart.color = Color.Lerp(Color.green, Color.white, currentState);
                }
            }
            yield return null;
        }
    }
}
