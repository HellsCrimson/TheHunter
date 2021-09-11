using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using baseRandom = System.Random;
using DiscordPresence;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    public Slider slider;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomnameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TMP_Text errorRoomName;

    public bool goSolo;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        PresenceManager.UpdatePresence(detail:"Dans le menu", state:"En ligne");
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        PhotonNetwork.ConnectUsingSettings();
        slider.value = 0.33f;
    }

    public override void OnConnectedToMaster()
    {
        slider.value = 0.66f;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined");
        slider.value = 1.0f;

        if (PhotonNetwork.LocalPlayer.NickName == "")
        {
            string nickname = "";
            bool hasNickname = AlreadyUser(ref nickname);
            
            if (!hasNickname)
            {
                MenuManager.Instance.OpenMenu("nicknameselect");
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = nickname;
                AudioManager.Instance.PlaySound("Theme");
                MenuManager.Instance.OpenMenu("main");
            }
        }
        else
        {
            AudioManager.Instance.PlaySound("Theme");
            MenuManager.Instance.OpenMenu("main");
        }
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Destroy(RoomManager.Instance);
        StartCoroutine(LoadAsynchronously(3));
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            errorRoomName.gameObject.SetActive(true);
            return;
        }

        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(roomNameInputField.text, option);
        MenuManager.Instance.OpenMenu("loading");
        
        PresenceManager.UpdatePresence(detail:"Dans un salon");
    }

    public override void OnJoinedRoom()
    {
        PresenceManager.UpdatePresence(detail:"Dans un salon");

        MenuManager.Instance.OpenMenu("waitingroom");
        roomnameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        CheckMaster();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        CheckMaster();
    }

    void CheckMaster()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        RoomSettingUser.Instance.isVisibleButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        RoomSettingUser.Instance.isntVisibleButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        
        RoomSettingUser.Instance.isJoinableButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        RoomSettingUser.Instance.isntJoinableButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed : " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame(string mapName)
    {
        if (PhotonNetwork.CurrentRoom.Players.Count < 2)
            return;
        
        AudioManager.Instance.StopSound("Theme");
        PresenceManager.UpdatePresence(detail:"En partie");
            
        if (!RoomSettingUser.Instance.isVisible)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        if (!RoomSettingUser.Instance.isJoinable)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        
        StartCoroutine(LoadAsynchronously(2));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        
        PhotonNetwork.LoadLevel(sceneIndex);
        var operation = PhotonNetwork.LevelLoadingProgress;

        MenuManager.Instance.OpenMenu("loading");
        
        while (operation < 1f)
        {
            operation = PhotonNetwork.LevelLoadingProgress;
            slider.value = operation;
            yield return null;
        }
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform tranf in roomListContent)
        {
            Destroy(tranf.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void GoSolo()
    {
        PresenceManager.UpdatePresence(detail:"En solo");
        AudioManager.Instance.StopSound("Theme");
        
        goSolo = true;
        Destroy(RoomManager.Instance);
        PhotonNetwork.Disconnect();
    }

    public bool AlreadyUser(ref string nickname)
    {
        if (File.Exists("./pseudo.tpas"))
        {
            using (StreamReader reader = new StreamReader("./pseudo.tpas"))
            {
                nickname = reader.ReadToEnd();
                if (nickname == "")
                {
                    nickname = "";
                    return false;
                }

                if (nickname.Length > 17)
                {
                    nickname = nickname.Substring(0, 17);
                    return true;
                }
                
                return true;
                
            }
        }
        return false;
    }
}
