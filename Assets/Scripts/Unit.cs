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
    public bool canFire = true;
    public bool canBeAttacked = true;
    protected SpriteRenderer[] spriteRenderers;
    protected float currentState;
    public Transform effectOverlayUI;
    [SerializeField] private float damageColorChangeSpeed = 4f;
    [SerializeField] private float deathColorChangeSpeed = 4f;
    public DamageNumberSO onHitDamageNumber;

    protected virtual void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    public virtual void TakeDamage(float damage)
    {
        if (canBeAttacked)
        {
            float damageToDeal = damage * defence;
            health -= damageToDeal;
            if (onHitDamageNumber)
            {
                onHitDamageNumber.Spawn(transform.position, damageToDeal);
            }
            if (health <= 0 && !isDying)
            {
                currentState = 0;
                isDying = true;
                OnKill();
                StartCoroutine(DeathAnimation());
            }
            else
            {
                currentState = 0;
                StartCoroutine(DamageAnimation());
            }
        }
    }
    public virtual void HealDamage(float healing)
    {
        health += healing;
        health = Mathf.Clamp(health, 0f, maxHealth);
        currentState = 0;
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
    protected IEnumerator DeathAnimation()
    {
        while (currentState < 1)
        {
            Debug.Log("STATE: DYING");
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
