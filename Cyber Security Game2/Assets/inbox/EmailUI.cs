using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EmailUI : MonoBehaviour
{
    public TMP_Text senderText;
    public TMP_Text subjectText;
    public Button trustButton;
    public Button reportButton;

    private EmailData emailData;
    private InboxManager inboxManager;
    public void Setup(EmailUI data, InboxManager manager)
    {
        emailData = data;
        inboxManager = manager;
        senderText.text = data.sender;
        subjectText.text = data.subject;
        trustButton.onClick.AddListener(() => inboxManager.OnTrustEmail(emailData));
        reportButton.onClick.AddListener(() => inboxManager.OnReportEmail(emailData));
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
