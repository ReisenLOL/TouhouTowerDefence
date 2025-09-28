using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;

public class MoveProjectileInfinite : Projectile
{
    private List<Unit> unitsHit = new();

    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        OnHitEffects(other);
    }

    protected override void OnHitEffects(Collider2D objectHit)
    {
        if (objectHit.TryGetComponent(out Enemy isEnemy))
        {
            if (unitsHit.Contains(isEnemy))
            {
                return;
            }
            unitsHit.Add(isEnemy);
            isEnemy.TakeDamage(damage);
        }
        if (hitParticles)
        {
            ParticleSystem newHitParticles = Instantiate(hitParticles);
            newHitParticles.transform.position = objectHit.transform.position;
            newHitParticles.transform.Lookat2D(transform.position);
        }
    }
}
