using System;
using Core.Extensions;
using UnityEngine;

public class RangedTowerSprite : MonoBehaviour
{
    public RangedTower thisTower;
    public Transform target;
    public ParticleSystem attackParticles;
    public Transform towerParticleLocation;
    public GameObject shadow;

    public void ProjectileAnimation()
    {
        if (target)
        {
            thisTower.FireProjectile(target); //THIS IS REALLY BAD.
            if (attackParticles)
            {
                ParticleSystem newParticles = Instantiate(attackParticles);
                newParticles.transform.position = towerParticleLocation.position;
            }
            thisTower.audioSource.PlayOneShot(thisTower.attackSound, thisTower.attackSoundVolume);
        }
        else
        {
            Debug.Log("YOU FUCKED UP DUDE");
        }
    }

    public void FinishDeploy()
    {
        shadow.SetActive(true);
    }
}
