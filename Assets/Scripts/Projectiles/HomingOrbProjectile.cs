using Unity.VisualScripting;
using UnityEngine;

public class HomingOrbProjectile : Projectile
{
    public float homingRange;
    public float timeUntilHoming;
    public LayerMask enemyLayer; //i'll have to change this so enemies can have homing projectiles, if ever!
    private float _time;
    private bool isHoming;
    public float damage;
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if (!isHoming)
        {
            _time += Time.deltaTime;
        }
        if (_time > timeUntilHoming && !isHoming)
        {
            _time = 0;
            speed *= 2;
            isHoming = true;
            target = DetectEnemies(homingRange);
        }
        if (isHoming && target)
        {
            RotateToTarget(target.position);
        }
        transform.Translate(Time.deltaTime * speed * Vector2.right);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.CompareTag("Tower") ||
            other.gameObject.layer == LayerMask.NameToLayer("Tower") && gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Unit>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    private Transform DetectEnemies(float radius)
    {
        return Physics2D.OverlapCircle(transform.position, radius, enemyLayer).gameObject.transform;
    }
}
