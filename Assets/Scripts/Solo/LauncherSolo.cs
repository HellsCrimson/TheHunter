using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LauncherSolo : MonoBehaviour
{
    
    public static LauncherSolo Instance;
    
    public Slider slider;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Launcher.Instance.goSolo)
            StartGame();
        else
            MenuManager.Instance.OpenMenu("main");
    }

    public void StartGame()
    {
        StartCoroutine(LoadAsynchronously(1));
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        MenuManager.Instance.OpenMenu("loading");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }

    public void Reconnect()
    {
        StartCoroutine(LoadAsynchronously(0));
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
