using Core.Extensions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float timeUntilAutoDestroy;
    public Transform target;
    public float damage;
    public ParticleSystem hitParticles;
    
    protected virtual void Start()
    {
        Destroy(gameObject, timeUntilAutoDestroy);
    }
    public void RotateToTarget(Vector2 direction)
    {
        transform.Lookat2D(direction);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.CompareTag("Tower") ||
            other.gameObject.layer == LayerMask.NameToLayer("Tower") && gameObject.CompareTag("Enemy"))
        {
            OnHitEffects(other);
            Destroy(gameObject);
        }
    }

    protected virtual void OnHitEffects(Collider2D objectHit)
    {
        if (hitParticles)
        {
            ParticleSystem newHitParticles = Instantiate(hitParticles);
            newHitParticles.transform.position = objectHit.transform.position;
            newHitParticles.transform.Lookat2D(transform.position);
        }
    }
}

