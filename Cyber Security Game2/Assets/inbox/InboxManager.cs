using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class InboxManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject emailPrefab;          // your prefab
    public Transform emailListParent;       // panel or ScrollView content
    public float emailInterval = 5f;        // seconds between emails

    [Header("Phishing Settings")]
    [Range(0f, 1f)]
    public float phishingChance = 0.3f;     // probability email is phishing
    [Range(1, 3)]
    public int phishingDifficulty = 1;      // more = more typos

    private float timer = 0f;
    private System.Random rng = new System.Random();

    // Email templates
    private List<EmailData> emailTemplates = new List<EmailData>()
    {
        new EmailData { sender = "Acme Bank", subject = "Verify your account", message = "Dear user, please verify your account here.", isPhishing = false },
        new EmailData { sender = "CloudBox", subject = "Invoice ready", message = "Hi, your invoice is ready to view.", isPhishing = false },
        new EmailData { sender = "Acme HR", subject = "New document available", message = "A new document is waiting for you.", isPhishing = false }
    };

    private static readonly Dictionary<char, char[]> adjacentKeys = new Dictionary<char, char[]>
    {
        {'a', new[]{'s','q','w','z'}},{'b', new[]{'v','g','h','n'}},{'c', new[]{'x','d','f','v'}},{'d', new[]{'s','e','r','f','c'}},
        {'e', new[]{'w','s','d','r'}},{'f', new[]{'d','r','t','g','v'}},{'g', new[]{'f','t','y','h','b'}},{'h', new[]{'g','y','u','j','n'}},
        {'i', new[]{'u','j','k','o'}},{'j', new[]{'h','u','i','k','m'}},{'k', new[]{'j','i','o','l'}},{'l', new[]{'k','o','p'}},
        {'m', new[]{'n','j','k'}},{'n', new[]{'b','h','j','m'}},{'o', new[]{'i','k','l','p'}},{'p', new[]{'o','l'}},
        {'q', new[]{'w','a'}},{'r', new[]{'e','d','f','t'}},{'s', new[]{'a','w','e','d','x','z'}},{'t', new[]{'r','f','g','y'}},
        {'u', new[]{'y','h','j','i'}},{'v', new[]{'c','f','g','b'}},{'w', new[]{'q','a','s','e'}},{'x', new[]{'z','s','d','c'}},
        {'y', new[]{'t','g','h','u'}},{'z', new[]{'a','s','x'}}
    };

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= emailInterval)
        {
            timer = 0f;
            SpawnEmail();
        }
    }

    void SpawnEmail()
{
    // Pick a random template
    EmailData template = emailTemplates[rng.Next(emailTemplates.Count)];

    // Create a new EmailData instance
    EmailData newEmail = new EmailData
    {
        sender = template.sender,
        subject = template.subject,
        message = template.message,
        isPhishing = rng.NextDouble() < phishingChance
    };

    // Inject typos if phishing
    if (newEmail.isPhishing)
    {
        int typoCount = phishingDifficulty;
        newEmail.subject = InjectTypos(newEmail.subject, typoCount);
        newEmail.message = InjectTypos(newEmail.message, typoCount + 1);
    }

    // Instantiate prefab as child of Content (emailListParent)
    GameObject go = Instantiate(emailPrefab, emailListParent);

    // Reset RectTransform so Layout Group can control positioning
    RectTransform rt = go.GetComponent<RectTransform>();
    rt.localScale = Vector3.one;
    rt.anchoredPosition = Vector2.zero;

    // Set TMP text
    TMP_Text senderText = go.transform.Find("Sender").GetComponent<TMP_Text>();
    TMP_Text subjectText = go.transform.Find("Subject").GetComponent<TMP_Text>();
    TMP_Text messageText = go.transform.Find("Message").GetComponent<TMP_Text>();

    senderText.text = newEmail.sender;
    subjectText.text = newEmail.subject;
    messageText.text = newEmail.message;

    // Assign EmailData for buttons
    EmailUI emailUI = go.GetComponent<EmailUI>();
    if (emailUI != null) emailUI.emailData = newEmail;
}


    // Inject typos in a string
    private string InjectTypos(string text, int count)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var tokens = Regex.Split(text, @"(\w+)");
        List<int> candidateIndices = new List<int>();
        for (int i = 0; i < tokens.Length; i++)
        {
            if (Regex.IsMatch(tokens[i], @"^\w+$"))
                candidateIndices.Add(i);
        }

        int tries = 0;
        while (count > 0 && candidateIndices.Count > 0 && tries < count * 5)
        {
            tries++;
            int idx = candidateIndices[rng.Next(candidateIndices.Count)];
            string word = tokens[idx];
            if (word.Length <= 1) continue;

            tokens[idx] = ApplyRandomTypo(word);
            count--;
        }

        return string.Join("", tokens);
    }

    private string ApplyRandomTypo(string word)
    {
        int type = rng.Next(3); // swap, delete, replace adjacent
        char[] arr = word.ToCharArray();

        switch (type)
        {
            case 0: // swap adjacent
                if (arr.Length < 2) return word;
                int i = rng.Next(arr.Length - 1);
                char t = arr[i]; arr[i] = arr[i + 1]; arr[i + 1] = t;
                return new string(arr);
            case 1: // delete
                arr = word.ToCharArray();
                int j = rng.Next(arr.Length);
                return word.Remove(j, 1);
            case 2: // replace with adjacent key
                for (int k = 0; k < arr.Length; k++)
                {
                    char c = char.ToLower(arr[k]);
                    if (adjacentKeys.ContainsKey(c))
                    {
                        var opts = adjacentKeys[c];
                        char repl = opts[rng.Next(opts.Length)];
                        arr[k] = char.IsUpper(arr[k]) ? char.ToUpper(repl) : repl;
                        break;
                    }
                }
                return new string(arr);
        }

        return word;
    }
}
