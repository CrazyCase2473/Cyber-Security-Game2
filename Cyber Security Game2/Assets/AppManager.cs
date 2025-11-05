using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    public GameObject currentApp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void OpenApp(GameObject app)
    {
        if (currentApp == app) return;
        if (currentApp != null)
            currentApp.SetActive(false);
        app.SetActive(true);
        currentApp = app;
    }
    public void CloseApp(GameObject app)
    {
        if (app == currentApp)
            currentApp = null;
        app.SetActive(false);
    }
}
