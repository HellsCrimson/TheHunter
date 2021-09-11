using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class HideSeek : MonoBehaviourPunCallbacks
{
    public static HideSeek Instance;
    [SerializeField] private PhotonView PV;

    public List<GameObject> Players;
    public List<GameObject> Prey;
    public List<GameObject> Hunter;

    [SerializeField] public int countdown;
    private Coroutine counter;
    private int waitCountDown;

    public bool ended = false;
    public bool waiting = false;

    [SerializeField] GameObject imageHunter;
    [SerializeField] TMP_Text preyWin;
    [SerializeField] TMP_Text hunterWin;
    [SerializeField] TMP_Text startCounterHunter;

    [SerializeField] private AudioSource audio;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (Instance != this)
            Destroy(Instance);
        else
        {
            Prey = new List<GameObject>();
            Hunter = new List<GameObject>();
            StartCoroutine(Wait());
        }
    }

    void Update()
    {
        DeleteMissingPlayer();
        AjoutJoueur();
        IsWaiting();
        EndGame();
    }

    void AjoutJoueur()
    {
        var players = FindObjectsOfType<PlayerController>();
        if (players.Length != Players.Count)
        {
            var toAdd = players.Where(player => !Players.Exists(player2 => player2 == player.gameObject));
            foreach (var add in toAdd)
            {
                Players.Add(add.gameObject);
            }
        }
    }

    void EndGame()
    {
        if (Prey.Count != 0 || Hunter.Count != 0 && !ended)
        {
            if (Hunter.Count == 0)
            {
                PV.RPC("End", RpcTarget.AllBuffered, 1);
            }

            else if (Prey.Count == 0)
            {
                PV.RPC("End", RpcTarget.AllBuffered, 0);
            }
            
            else if (CheckHunterAmo())
            {
                PV.RPC("End", RpcTarget.AllBuffered, 1);
            }
        }
    }

    bool CheckHunterAmo()
    {
        for (int i = 0; i < Hunter.Count; i++)
        {
            if (Hunter[i].GetComponent<MagazineUI>().curMagazine == 0 &&
                Hunter[i].GetComponent<MagazineUI>().totalMagazine == 0)
            {
                return true;
            }
        }
        return false;
    }

    void AddPlayers()
    {
        var players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            if (player.name.Contains("PlayerControllerHunter"))
            {
                Hunter.Add(player.gameObject);
            }
            else if (player.name.Contains("PlayerControllerPrey"))
            {
                Prey.Add(player.gameObject);
            }
        }
    }

    IEnumerator Wait()
    {
        waiting = true;
        waitCountDown = 30;
        while (waitCountDown > 0)
        {
            if (waitCountDown == 15)
                AddPlayers();
            startCounterHunter.text = waitCountDown.ToString();
            yield return new WaitForSeconds(1f);
            waitCountDown--;
        }
        if (PV.IsMine && PhotonNetwork.IsMasterClient)
        {
            counter = StartCoroutine(Countdown());
        }
        waiting = false;
    }

    IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            Debug.Log(countdown);
            yield return new WaitForSeconds(1f);
            countdown--;
            if (countdown % 3 == 0)
            {
                HunterLosePV();
            }
        }
        
        PV.RPC("End", RpcTarget.AllBuffered, 1);
    }

    public void HunterLosePV()
    {
        for (int i = 0; i < Hunter.Count; i++)
        {
            Hunter[i].GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    public void DeleteMissingPlayer()
    {
        Hunter.RemoveAll(hunter => hunter == null);
        Prey.RemoveAll(prey => prey == null);
        Players.RemoveAll(player => player == null);
    }

    [PunRPC]
    void End(int i)
    {
        if (counter != null)
            StopCoroutine(counter);
        ended = true;
        
        if (i == 0)
        {
            audio.Play();
            hunterWin.gameObject.SetActive(true);
            StartCoroutine(BackMainMenu());
        }
        else
        {
            audio.Play();
            preyWin.gameObject.SetActive(true);
            StartCoroutine(BackMainMenu());
        }
    }

    void IsWaiting()
    {
        foreach (var player in Players)
        {
            if (player.name.Contains("PlayerControllerHunter"))
            {
                if (waiting && player.GetComponent<PhotonView>().IsMine)
                {
                    player.GetComponent<PauseMenuMulti>().healthUi.SetActive(false);
                    player.GetComponent<PauseMenuMulti>().ammoUI.SetActive(false);
                    player.GetComponent<PauseMenuMulti>().reticule.SetActive(false);
                    imageHunter.SetActive(true);
                    startCounterHunter.gameObject.SetActive(true);
                }
                else if (!waiting && player.GetComponent<PhotonView>().IsMine)
                {
                    player.GetComponent<PauseMenuMulti>().healthUi.SetActive(true);
                    player.GetComponent<PauseMenuMulti>().ammoUI.SetActive(true);
                    player.GetComponent<PauseMenuMulti>().reticule.SetActive(true);
                    imageHunter.SetActive(false);
                    startCounterHunter.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator BackMainMenu()
    {
        yield return new WaitForSeconds(10f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
}
