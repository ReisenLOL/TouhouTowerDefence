using System;
using Core.Extensions;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }

    public void RotateToTarget(Vector2 direction)
    {
        transform.Lookat2D(direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.CompareTag("Tower") ||
            other.gameObject.layer == LayerMask.NameToLayer("Tower") && gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
