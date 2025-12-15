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
    public int maxBugs = 5;
    public float baseSpawnRate = 1.5f;
    public float bugSpeedIncrease = 0.2f;

    [Header("Score")]
    public int score = 0;
    public TMP_Text scoreText;

    [Header("Questions")]
    public QuestionManager questionManager;
    public GameObject questionPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text levelText;
    public TMP_Text bugCountText;
    private int stepsUntilQuestion = 3;

    [Header("Level Timing")]
    public float baseTimePerLevel = 12f;
    public float timeIncreasePerLevel = 0.2f;

    [Header("Difficulty")]
    public int difficultyLevel = 1;
    private float difficultyTimer = 0f;

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip bugSquashClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip gameOverClip;

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
        InvokeRepeating(nameof(SpawnBug), 0f, baseSpawnRate);
    }

    void Update()
    {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= baseTimePerLevel)
        {
            difficultyTimer = 0f;
            IncreaseDifficulty();
        }

        UpdateInfoUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        stepsUntilQuestion--;

        if (stepsUntilQuestion <= 0)
        {
            stepsUntilQuestion = Mathf.Max(1, 3 - difficultyLevel / 2);
            ShowQuestion();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void SpawnBug()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        int currentBugs = GameObject.FindGameObjectsWithTag("Bug").Length;
        if (currentBugs >= maxBugs)
        {
            LoseGame();
            return;
        }

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (spawn == null) return;

        GameObject bug = Instantiate(bugPrefab, spawn.position, Quaternion.identity);

        Bug bugScript = bug.GetComponent<Bug>();
        if (bugScript != null)
            bugScript.speed += difficultyLevel * bugSpeedIncrease;
    }

    public void PlayBugSquash()
    {
        if (sfxSource != null && bugSquashClip != null)
            sfxSource.PlayOneShot(bugSquashClip);
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

        if (sfxSource != null && correctClip != null)
            sfxSource.PlayOneShot(correctClip);
    }

    void Wrong()
    {
        Time.timeScale = 1;

        if (sfxSource != null && wrongClip != null)
            sfxSource.PlayOneShot(wrongClip);

        LoseGame();
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;

        CancelInvoke(nameof(SpawnBug));
        float newRate = Mathf.Max(0.2f, baseSpawnRate * Mathf.Pow(0.95f, difficultyLevel));
        InvokeRepeating(nameof(SpawnBug), 0f, newRate);

        maxBugs = Mathf.FloorToInt(5 * Mathf.Pow(1.1f, difficultyLevel));
        baseTimePerLevel += timeIncreasePerLevel;
    }

    void LoseGame()
    {
        Time.timeScale = 1;
        Cursor.visible = true;

        PlayerPrefs.SetInt("FinalScore", score);

        if (sfxSource != null && gameOverClip != null)
            sfxSource.PlayOneShot(gameOverClip);

        Invoke(nameof(LoadEndMenu), 0.5f);
    }

    void LoadEndMenu()
    {
        SceneManager.LoadScene("End Menu");
    }

    void UpdateInfoUI()
    {
        levelText.text = "Level: " + difficultyLevel;

        int currentBugs = GameObject.FindGameObjectsWithTag("Bug").Length;
        bugCountText.text = "Bugs: " + currentBugs + " / " + maxBugs;
    }
}
