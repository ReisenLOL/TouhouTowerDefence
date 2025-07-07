using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnKill();
        }
    }

    protected virtual void OnKill()
    {
        
    }
}
