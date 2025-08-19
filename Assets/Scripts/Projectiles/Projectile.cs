using Core.Extensions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float timeUntilAutoDestroy;
    public Transform target;
    
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
            Destroy(gameObject);
        }
    }
}

