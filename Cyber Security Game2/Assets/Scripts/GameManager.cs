using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Bug Settings")]
    public GameObject bugPrefab;
    public Transform[] spawnPoints;
    public int maxBugs = 10;
    public float baseSpawnRate = 1.5f;

    [Header("Score")]
    public int score = 0;
    public TMP_Text scoreText;

    [Header("Questions")]
    public QuestionManager questionManager;
    public GameObject questionPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    private int stepsUntilQuestion = 3;

    [Header("Difficulty")]
    public int difficultyLevel = 1;
    public float timePerLevel = 30f;
    private float difficultyTimer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        questionPanel.SetActive(false);

        InvokeRepeating("SpawnBug", 0f, baseSpawnRate);
    }

    void Update()
    {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= timePerLevel)
        {
            difficultyTimer = 0f;
            IncreaseDifficulty();
        }
    }


    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        stepsUntilQuestion--;

        if (stepsUntilQuestion <= 0)
        {
            stepsUntilQuestion = Mathf.Max(1, 3 - difficultyLevel);
            ShowQuestion();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void SpawnBug()
    {
        int currentBugs = GameObject.FindGameObjectsWithTag("Bug").Length;

        if (currentBugs >= maxBugs)
        {
            LoseGame();
            return;
        }

        if (spawnPoints.Length == 0) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(bugPrefab, spawn.position, Quaternion.identity);
    }

    void ShowQuestion()
    {
        Time.timeScale = 0;
        Cursor.visible = true;

        var q = questionManager.GetRandomQuestion();
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];

            if (i == q.correctIndex)
                answerButtons[i].onClick.AddListener(Correct);
            else
                answerButtons[i].onClick.AddListener(Wrong);
        }

        questionPanel.SetActive(true);
    }

    void Correct()
    {
        questionPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    void Wrong()
    {
        Time.timeScale = 1;
        LoseGame();
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;

        // Faster bug spawns
        CancelInvoke("SpawnBug");
        float newRate = Mathf.Max(0.4f, baseSpawnRate - difficultyLevel * 0.15f);
        InvokeRepeating("SpawnBug", 0f, newRate);

        // Fewer allowed bugs
        maxBugs = Mathf.Max(4, maxBugs - 1);

        Debug.Log("Difficulty increased to " + difficultyLevel);
    }

    void LoseGame()
    {
        Time.timeScale = 1;
        Cursor.visible = true;
        SceneManager.LoadScene("End Menu");
    }
}
