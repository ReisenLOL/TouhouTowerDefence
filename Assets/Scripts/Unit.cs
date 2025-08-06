using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float defence = 1f;
    public float attackModifier = 1f;
    public virtual void TakeDamage(float damage)
    {
        health -= damage * defence;
        if (health <= 0)
        {
            OnKill();
        }
    }

    protected virtual void OnKill()
    {
        
    }
}
