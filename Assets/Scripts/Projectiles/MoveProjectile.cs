using System;
using Core.Extensions;
using UnityEngine;

public class MoveProjectile : Projectile
{
    public Rigidbody2D rb;
    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = (target.transform.position - transform.position).normalized * speed;
    }
    protected override void OnHitEffects(Collider2D objectHit)
    {
        base.OnHitEffects(objectHit);
        if (objectHit.TryGetComponent(out Unit isRangedTower))
        {
            isRangedTower.TakeDamage(damage, willBypassDefense);
        }
        else if (objectHit.TryGetComponent(out TowerBlockingCollision isMeleeTower))
        {
            isMeleeTower.thisTower.TakeDamage(damage, willBypassDefense);
        }
        Destroy(gameObject);
    }
}
