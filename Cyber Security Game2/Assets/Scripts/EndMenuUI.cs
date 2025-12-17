using UnityEngine;
using TMPro;

public class EndMenuUI : MonoBehaviour
{
    public TMP_Text finalScoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        finalScoreText.text = "Score: " + finalScore;
    }
}
