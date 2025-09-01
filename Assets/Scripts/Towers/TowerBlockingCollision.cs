using System.Linq;
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
        if (other.gameObject.layer == 9 && !thisTower.stats.isCliffTower)
        {
            foreach (Enemy findBlockedEnemy in thisTower.currentlyBlocking.ToList())
            {
                if (!findBlockedEnemy)
                {
                    thisTower.currentlyBlocking.Remove(findBlockedEnemy);
                }
            }
            Enemy foundUnit = other.GetComponent<Enemy>();
            if (thisTower.currentlyBlocking.Count < thisTower.stats.blockAmount && !thisTower.currentlyBlocking.Contains(foundUnit))
            {
                thisTower.currentlyBlocking.Add(foundUnit);
                foundUnit.canMove = false;
            }
        }
    }
}
