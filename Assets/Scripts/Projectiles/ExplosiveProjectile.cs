using Unity.Cinemachine;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    //only targets enemies rn
    public Rigidbody2D rb;
    public float radius;
    public float blastDamage;
    public LayerMask enemyLayers;
    public ParticleSystem blastParticles;
    public CinemachineImpulseSource impulseSource;
    public float cameraShakeForce;
    private void FixedUpdate()
    {
        rb.linearVelocity = (target.transform.position - transform.position).normalized * speed;
    }
    protected override void OnHitEffects(Collider2D objectHit)
    {
        base.OnHitEffects(objectHit);
        if (objectHit.TryGetComponent(out Unit isRangedTower))
        {
            isRangedTower.TakeDamage(damage);
        }
        else if (objectHit.TryGetComponent(out TowerBlockingCollision isMeleeTower))
        {
            isMeleeTower.thisTower.TakeDamage(damage);
        }
        Explode();
        Instantiate(blastParticles, transform.position, blastParticles.transform.rotation);
        Destroy(gameObject);
    }

    private void Explode()
    {
        impulseSource.DefaultVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        impulseSource.GenerateImpulse(cameraShakeForce);
        Collider2D[] enemiesInBlast = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
        foreach (Collider2D enemy in enemiesInBlast)
        {
            if (enemy.TryGetComponent(out Enemy isEnemy))
            {
                isEnemy.TakeDamage(blastDamage);
            }
        }
    }
}
