using DamageNumbersPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Number", menuName ="Damage Number")]
public class DamageNumberSO : ScriptableObject
{
    [SerializeField] DamageNumber numberPrefab;
    public void Spawn(Vector2 position, float value)
    {
        if (numberPrefab != null)
        {
            numberPrefab.Spawn(position, value);
        }
    }
    public void SpawnText(Vector2 position, string text)
    {
        if (numberPrefab != null)
        {
            numberPrefab.Spawn(position, text);
        }
    }
}
