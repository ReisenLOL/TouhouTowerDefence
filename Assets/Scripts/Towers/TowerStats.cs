using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Stats", menuName = "Tower/TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("DEFAULT IS FOCUSED")]
    public float damage;
    public float scatteredDamageModifier;
    public float fireRate;
    public float scatteredFireRateModifier;
    public int blockAmount;
    public bool isCliffTower;
    public bool canDetectAir;
    [Header("PLAYER TOWER STATS")] 
    public float playerFireRate;
    public float playerDamage;
    public float playerProjectileSpeed;
    public float playerMoveSpeed;
}
