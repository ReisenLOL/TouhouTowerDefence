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
        if (thisUnit.currentWaypoint == thisUnit.selectedSpawnAndMovement.waypoints.Count)
        {
            FindAnyObjectByType<GameManager>().TakeDamage();
            Destroy(thisUnit.gameObject);
        }
    }
}
