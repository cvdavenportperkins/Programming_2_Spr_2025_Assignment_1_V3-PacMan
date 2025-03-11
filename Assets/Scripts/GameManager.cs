using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Singleton instance
    private SoundManager Instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI youWinText;
    public TextMeshProUGUI gameOverText;
    public int score = 0;
    public float timer;
    private bool isGameOver = false;
    public bool isPlayerTurboActive = false; //power up state
    public float gameTime = 80f;
    private int health = 3;
    public Transform playerSpawn;
    public GameObject playerPrefab;

    public Ghosts[] ghosts;
    public Transform pellet;
    public Transform powerpellet;
    public List<GameObject> pellets = new List<GameObject>();

    public GameObject healthImage1;
    public GameObject healthImage2;
    public GameObject healthImage3;



    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            instance = this; // Set instance to this
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // Make GameManager persistent across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        timer = gameTime;
        UpdateScore();
        UpdateTimer();
        SoundManager.Instance.PlaySound(SoundManager.Instance.GameStartSFX);
        GameObject[] pelletsArray = GameObject.FindGameObjectsWithTag("Pellet");
        pellets.AddRange(pelletsArray);

        health = 3;
        InstantiatePlayer();
        gameOverText.gameObject.SetActive(false);
        youWinText.gameObject.SetActive(false);
        healthImage1.SetActive(true);
        healthImage2.SetActive(true);
        healthImage3.SetActive(true);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;
            UpdateTimer();

            if(timer <= 0)
            {
                timer = 0;
                GameOver();
            }
        }
    }

    private void InstantiatePlayer()
    {
        Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
        CheckVictoryCondition();
    }

    public void SubtractScore(int points)
    {
        score -= points;
        UpdateScore();
    }
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateTimer()
    {
        timerText.text = "Time: " + Mathf.FloorToInt(timer);
    }

    public void DecreaseHealth()
    {
        health--;
        SoundManager.Instance.PlaySound(SoundManager.Instance.PlayerHitSFX);
        Debug.Log("Health: " + health);

        if (health == 2)
        {
            healthImage3.SetActive(false);
        }
        else if (health == 1)
        {
            healthImage2.SetActive(false);
        }
        else if (health <= 0)
        {
            healthImage1.SetActive(false);
            GameOver();
        }
    }


    public void GameOver()
    {

        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        // Show game over UI or reload the scene
        Debug.Log("Game Over!");
        StartCoroutine(RestartGameAfterDelay());
    }

    public void Victory()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            youWinText.gameObject.SetActive(true);
            Debug.Log("Victory");
            StartCoroutine(RestartGameAfterDelay());
        }
    }

    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        RestartGame();
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        timer = gameTime;
        isGameOver = false;
        score = 0;
        isPlayerTurboActive = false;
        UpdateScore();
        UpdateTimer();
        
        pellets.Clear();
        GameObject[] pelletsArray = GameObject.FindGameObjectsWithTag("Pellet");
        pellets.AddRange(pelletsArray);
    }

    public void ActivatePlayerTurbo(float duration)
    {
        isPlayerTurboActive = true;
        PacManController.instance.speed = 8f;
        StartCoroutine(DeactivatePlayerTurboAfterDuration(duration));
    }

    private System.Collections.IEnumerator DeactivatePlayerTurboAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPlayerTurboActive = false;
    }

    private void CheckVictoryCondition()
    {
        if (pellets.Count == 0)
        {
            Victory();
        }
    }

    public void CollectPellet(GameObject pellet)
    {
        pellets.Remove(pellet);
    }
}


