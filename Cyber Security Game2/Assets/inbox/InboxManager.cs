using UnityEngine;

public class InboxManager : MonoBehaviour
{
    public GameObject emailPrefab;
    public Transform contentParent;
    public EmailData[] allEmails;
    void Start()
    {
        foreach (EmailData data in allEmails)
        {
            GameObject emailObj = Instantiate(emailPrefab, contentParent);
            EmailUI ui = emailObj.GetComponent<EmailUI>();
            ui.Setup(data, this);
        }
    }
    public void OnTrustEmail(EmailData data)
    {
        if (data.isPhishing)
        {
            Debug.Log("Wrong! You trusted a phishing email: " + data.subject);
            // add consequence later
        }
        else
        {
            Debug.Log("Correct! Email was safe: " + data.subject);
            //  Reward points or progress
        }
    }
    public void OnReportEmail(EmailData data)
    {
        if (data.isPhishing)
        {
            Debug.Log("Good catch! You reported a phishing email: " + data.subject);
            // Add reward later
        }
        else
        {
            Debug.Log("Oops! That was a real email: " + data.subject);
            // add consequence later
        }
    }

    void Update()
    {
        
    }
}
