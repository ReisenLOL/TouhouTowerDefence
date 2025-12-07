using System;
using UnityEngine;

public class PlayerTowerController : Tower
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public Projectile projectile;
    private Camera cam;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        cam = Camera.main;
    }

    // Update is called once per frame
    protected override void Update()
    {
        HandleMovement();
        HandleAttack();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * stats.playerMoveSpeed;
    }

    private void HandleAttack()
    {
        currentFiringTime += Time.deltaTime;
        if (Input.GetMouseButton(0) && currentFiringTime > stats.playerFireRate)
        {
            FireProjectile(cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10)));
            audioSource.PlayOneShot(attackSound, attackSoundVolume);
            animator.SetTrigger(attackAnimParam);
            currentFiringTime = 0;
        }
    }

    private void HandleMovement()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public virtual void FireProjectile(Vector3 direction)
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.damage = stats.playerDamage * attackModifier;
        newProjectile.speed = stats.playerProjectileSpeed;
        newProjectile.RotateToTarget(direction);
    }
}
