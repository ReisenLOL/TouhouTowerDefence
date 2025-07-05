using UnityEngine;

public class EnemyCenterWaypoint : MonoBehaviour
{
    public Enemy thisUnit;
    void Start()
    {
        thisUnit = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        thisUnit.currentWaypoint++;
    }
}
