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
    public int rating;
    public int currentEnemyCount;
    public int totalEnemyCount;
    public TextMeshProUGUI gameStateText;
    public TextMeshProUGUI ratingText;
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip pauseSound;
    public float pauseVolume;
    public float damageVolume;
    public bool speedUp;
    public bool paused;
    public Transform enemyBar;
    public Transform livesBar;
    public GameObject pauseUI;

    public void TakeDamage(int damage = 1)
    {
        lives -= damage;
        audioSource.PlayOneShot(damageSound, damageVolume);
        UpdateLivesUI();
        if (lives <= 0)
        {
            gameOver = true;
            EndGame();
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        if (!gameOver)
        {
            rating++;
            if (lives == maxLives)
            {
                rating++;
            }
            if (lives <= maxLives / 2)
            {
                rating++;
            }
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

    public void ToggleSpeed()
    {
        if (speedUp)
        {
            speedUp = false;
            Time.timeScale = 1;
        }
        else
        {
            speedUp = true;
            Time.timeScale = 2;
        }
    }

    public void TogglePause()
    {
        if (paused)
        {
            paused = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        else
        {
            paused = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            audioSource.PlayOneShot(pauseSound, pauseVolume);
        }
    }
    public void UpdateEnemyCount()
    {
        if (currentEnemyCount == totalEnemyCount)
        {
            EndGame();
        }
        enemyCountText.text = currentEnemyCount + "/" + totalEnemyCount;
        enemyBar.transform.localScale = new Vector3((float)currentEnemyCount/totalEnemyCount, 1, 1);
    }
    private void UpdateLivesUI()
    {
        livesText.text = lives + "/" + maxLives; 
        livesBar.transform.localScale = new Vector3((float)lives/maxLives, 1, 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Prototype");
    }

    public void ReturnGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
