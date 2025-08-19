using System;
using Core.Extensions;
using UnityEngine;

public class MoveProjectile : Projectile
{
    private void Update()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }
}
