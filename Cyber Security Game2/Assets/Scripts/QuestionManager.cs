using UnityEngine;

[System.Serializable]
public class CyberQuestion
{
    public string question;
    public string[] answers; 
    public int correctIndex; 
}

public class QuestionManager : MonoBehaviour
{
    public CyberQuestion[] questions;

    public CyberQuestion GetRandomQuestion()
    {
        int r = Random.Range(0, questions.Length);
        return questions[r];
    }
}
