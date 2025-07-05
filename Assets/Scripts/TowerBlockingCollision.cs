using UnityEngine;

public class TowerBlockingCollision : MonoBehaviour
{
    public Tower thisTower;
    void Start()
    {
        thisTower = GetComponentInParent<Tower>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && !thisTower.isCliffTower)
        {
            Enemy foundUnit = other.GetComponent<Enemy>();
            if (thisTower.currentlyBlocking.Count < thisTower.blockAmount && !thisTower.currentlyBlocking.Contains(foundUnit))
            {
                thisTower.currentlyBlocking.Add(foundUnit);
                foundUnit.canMove = false;
            }
        }
    }
}
