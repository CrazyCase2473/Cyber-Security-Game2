using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject bugPrefab;
    public Transform[] spawnPoints;

    public int score = 0;
    public TMPro.TMP_Text scoreText;


    public QuestionManager questionManager;

    // UI
    public GameObject questionPanel;
    public TMPro.TMP_Text questionText;
    public Button[] answerButtons;

    int stepsUntilQuestion = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        InvokeRepeating("SpawnBug", 0, 1.5f);
        questionPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        stepsUntilQuestion--;
        if (stepsUntilQuestion <= 0)
        {
            stepsUntilQuestion = 3;
            ShowQuestion();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void SpawnBug()
    {
        if (spawnPoints.Length == 0) return;
        int r = Random.Range(0, spawnPoints.Length);
        Instantiate(bugPrefab, spawnPoints[r].position, Quaternion.identity);
    }

    // QUESTIONS
    void ShowQuestion()
    {
        Cursor.visible = true;
        Time.timeScale = 0;

        var q = questionManager.GetRandomQuestion();
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];


            answerButtons[i].onClick.RemoveAllListeners();

            if (i == q.correctIndex)
            {
                answerButtons[i].onClick.AddListener(() => Correct());
            }
            else
            {
                answerButtons[i].onClick.AddListener(() => Wrong());
            }
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
        Debug.Log("You failed. Nice job.");
        // Here you can add: reset score, reload scene, whatever
    }
}
