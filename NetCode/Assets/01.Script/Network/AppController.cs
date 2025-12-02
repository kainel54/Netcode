using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartApp();
    }


    public void StartApp()
    {
        GameObject appClient = new GameObject();
        appClient.name = "AppClient";
        appClient.transform.SetParent(transform);
        AppClient client = appClient.AddComponent<AppClient>();

        GameObject appHost = new GameObject();
        appHost.name = "AppHost";
        appHost.transform.SetParent(transform);
        AppHost host = appHost.AddComponent<AppHost>();

        SceneManager.LoadScene(SceneNames.MenuScene);
    }
}

