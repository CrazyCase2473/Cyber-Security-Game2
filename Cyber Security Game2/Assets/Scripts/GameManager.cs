using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay")]
    public GameObject bugPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f;

    [Header("UI")]
    public TMP_Text scoreText;
    public GameObject questionPanel; 

    [Header("Game State")]
    public int score = 0;
    private bool gameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreText();
        InvokeRepeating("SpawnBug", 0f, spawnInterval);
    }

    // spawn but at spawn points
    void SpawnBug()
    {
        if (gameOver || spawnPoints.Length == 0) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(bugPrefab, spawn.position, Quaternion.identity);
    }

    // when bug gets killed
    public void BugSquashed(Bug bug)
    {
        score++;
        UpdateScoreText();

        // chance to ask question
        if(Random.value < 0.3f) // 30%
            ShowQuestion();
    }

    
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // show question and pause game
    public void ShowQuestion()
    {
        if (questionPanel != null)
        {
            questionPanel.SetActive(true);
            Time.timeScale = 0f; // pauses
        }
    }

    //called by buttons on questions
    public void AnswerQuestion(bool correct)
    {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        Time.timeScale = 1f;

        if(!correct)
            EndGame();
    }

    void EndGame()
    {
        gameOver = true;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(score > highScore)
            PlayerPrefs.SetInt("HighScore", score);

        Debug.Log("Game Over! Score: " + score + " | High Score: " + PlayerPrefs.GetInt("HighScore"));
        Time.timeScale = 0f; 
    }
}
