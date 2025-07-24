using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI gameStateText;
    public TextMeshProUGUI ratingText;

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
        endGameUI.SetActive(true);
        ratingText.text = "Rating: " + rating;
        if (gameOver)
        {
            gameStateText.text = "Failed...";
        }
        else
        {
            gameStateText.text = "Completed!";
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

    public void RestartGame()
    {
        SceneManager.LoadScene("Prototype");
    }

    public void ReturnGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
