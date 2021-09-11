using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuMulti : MonoBehaviour
{
    public static PauseMenuMulti Instance;
    
    public static bool GameIsPaused;

    public GameObject pauseMenuUi;
    public GameObject healthUi;
    public GameObject ammoUI;
    public GameObject optionMenu;
    public GameObject reticule;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void DesactivateUI()
    {
        pauseMenuUi.gameObject.SetActive(false);
        healthUi.gameObject.SetActive(false);
        ammoUI.gameObject.SetActive(false);
        optionMenu.SetActive(false);
        reticule.gameObject.SetActive(false);
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        healthUi.SetActive(true);
        ammoUI.SetActive(true);
        optionMenu.SetActive(false);
        reticule.SetActive(true);
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        healthUi.SetActive(false);
        ammoUI.SetActive(false);
        reticule.SetActive(false);
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToOption()
    {
        pauseMenuUi.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void ToPauseMenu()
    {
        pauseMenuUi.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void ToMainMenu()
    {
        Destroy(RoomManager.Instance);
        PhotonNetwork.LeaveRoom();
        StartCoroutine(LoadAsynchronously(0));
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        MenuManager.Instance.OpenMenu("loading");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Launcher.Instance.slider.value = progress;
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
