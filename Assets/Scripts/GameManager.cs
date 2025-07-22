using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives = 5;
    public int maxLives = 5;
    public bool gameOver = false;
    [Header("[CACHE]")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI enemyCountText;
    public GameObject endGameUI;
    public int rating = 0;
    public int currentEnemyCount;
    public int totalEnemyCount;

    public void TakeDamage(int damage = 1)
    {
        lives -= damage;
        UpdateLivesUI();
        if (lives >= 0)
        {
            gameOver = true;
        }
    }

    public void EndGame()
    {
        endGameUI.SetActive(true);
        if (!gameOver)
        {
            rating++;
        }
        if (lives == maxLives)
        {
            rating++;
        }
        if (lives <= maxLives / 2)
        {
            rating++;
        }
    }
    public void UpdateEnemyCountUI()
    {
        enemyCountText.text = "Enemies: " + currentEnemyCount + "/" + totalEnemyCount;
    }
    private void UpdateLivesUI()
    {
        livesText.text = "Lives: " + lives + "/" + maxLives; 
    }
}
