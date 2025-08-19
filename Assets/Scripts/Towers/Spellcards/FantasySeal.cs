using UnityEngine;

public class FantasySeal : Spellcard
{
    public float damage;
    public Transform[] firingPositions;
    public HomingOrbProjectile projectile;
    protected override void SpellCardEffects()
    {
        for (int i = 0; i < firingPositions.Length; i++)
        {
            FireProjectile(firingPositions[i]);
        }
    }
    private void FireProjectile(Transform spawnPosition)
    {
        HomingOrbProjectile spawnedAttack = Instantiate(projectile, transform.position, projectile.transform.rotation);
        spawnedAttack.RotateToTarget(spawnPosition.position);
        spawnedAttack.damage = thisTower.stats.damage * thisTower.attackModifier;
        spawnedAttack.tag = thisTower.tag;
    }
    protected override void Update()
    {
        base.Update();
    }
}
