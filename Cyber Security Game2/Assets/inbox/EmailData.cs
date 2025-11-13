using UnityEngine;

[CreateAssetMenu(fileName = "EmailData", menuName = "Email System/EmailData")]
public class EmailData : ScriptableObject
{
    public string sender;
    public string subject;
    [TextArea(3, 10)] public string message;
    public bool isPhishing;
}
