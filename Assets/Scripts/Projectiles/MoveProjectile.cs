using System;
using Core.Extensions;
using UnityEngine;

public class MoveProjectile : Projectile
{
    public Rigidbody2D rb;
    private void FixedUpdate()
    {
        rb.linearVelocity = (target.transform.position - transform.position).normalized * speed;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.CompareTag("Tower") ||
            other.gameObject.layer == LayerMask.NameToLayer("Tower") && gameObject.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Unit isRangedTower))
            {
                isRangedTower.TakeDamage(damage);
            }
            else if (other.TryGetComponent(out TowerBlockingCollision isMeleeTower))
            {
                isMeleeTower.thisTower.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
