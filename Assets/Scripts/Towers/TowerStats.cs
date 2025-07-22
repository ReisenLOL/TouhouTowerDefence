using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Stats", menuName = "Tower/TowerStats")]
public class TowerStats : ScriptableObject
{
    public float damage;
    public float fireRate;
    public int blockAmount;
    public bool isCliffTower;
    public bool canDetectAir;
}
