using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = System.Random;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    
    GameObject controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }


    void CreateController()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Random rng = new Random();
            int hunterIndex = rng.Next(PhotonNetwork.PlayerList.Length);
            PV.RPC("IsHunter", RpcTarget.All, hunterIndex);
        }
    }
    
    [PunRPC]
    public void IsHunter(int hunterIndex)
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        
        if (PhotonNetwork.PlayerList[hunterIndex] == PhotonNetwork.LocalPlayer)
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerHunter"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        }
        else
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerPrey"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        }
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateSpectator();
    }
    
    void CreateSpectator()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spectator"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }
}
