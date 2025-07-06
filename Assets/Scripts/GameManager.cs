using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives = 5;
    public bool gameOver = false;

    public void TakeDamage(int damage = 1)
    {
        lives -= damage;
        if (lives >= 0)
        {
            gameOver = true;
        }
    }
}
