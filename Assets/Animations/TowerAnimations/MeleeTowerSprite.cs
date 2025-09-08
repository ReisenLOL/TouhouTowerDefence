using UnityEngine;

public class MeleeTowerSprite : MonoBehaviour
{
    public MeleeTower thisTower;
    public ParticleSystem attackParticles;
    public Transform towerParticleLocation;
    public GameObject shadow;

    public void MeleeAnimation()
    {
        thisTower.DealDamage();
        if (attackParticles)
        {
            ParticleSystem newParticles = Instantiate(attackParticles);
            newParticles.transform.position = towerParticleLocation.position;
        }
        thisTower.audioSource.PlayOneShot(thisTower.attackSound, thisTower.attackSoundVolume);
    }
    public void FinishDeploy()
    {
        shadow.SetActive(true);
    }
}
