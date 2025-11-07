using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmailUI : MonoBehaviour
{
    public EmailData emailData;       // stores this email's info
    public Button TrustButton;        // assign in prefab
    public Button ReportButton;       // assign in prefab

    private void Start()
    {
        TrustButton.onClick.AddListener(OnTrustClicked);
        ReportButton.onClick.AddListener(OnReportClicked);
    }

    private void OnTrustClicked()
    {
        if (emailData.isPhishing)
            Debug.Log("Oops! That was phishing!");
        else
            Debug.Log("Correct! This email is safe.");
    }

    private void OnReportClicked()
    {
        if (emailData.isPhishing)
            Debug.Log("Correct! You reported phishing.");
        else
            Debug.Log("Oops! This email was safe.");
    }
}
