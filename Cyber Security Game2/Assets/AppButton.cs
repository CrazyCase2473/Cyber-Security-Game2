using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;
public class AppButton : MonoBehaviour
{
    public GameObject appPanel;
    public bool closeOtherApps = true;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenApp);
    }

    void OpenApp()
    {
        if(appPanel == null)
        {
            Debug.LogWarning("no app panel assigned" + gameObject.name);
            return;
        }
        if (closeOtherApps)
        {
            foreach (Transform child in appPanel.transform.parent)
                child.gameObject.SetActive(false);
        }
        appPanel.SetActive(true);
    }
}
