using UnityEngine;

public class HealerTowerSprite : RangedTowerSprite
{
    public Tower healingTarget;
    public void HealAnimation()
    {
        if (healingTarget)
        {
            healingTarget.HealDamage(thisTower.stats.damage);
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
}
