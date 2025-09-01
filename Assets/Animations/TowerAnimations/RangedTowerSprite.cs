using System;
using UnityEngine;

public class RangedTowerSprite : MonoBehaviour
{
    public RangedTower thisTower;

    public void ProjectileAnimation()
    {
        if (thisTower.closestEnemy)
        {
            thisTower.FireProjectile(thisTower.closestEnemy.transform); //THIS IS REALLY BAD.
            thisTower.audioSource.PlayOneShot(thisTower.attackSound);
        }
        else
        {
            Debug.Log("YOU FUCKED UP DUDE");
        }
    }
}
