using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerTransform : MonoBehaviourPunCallbacks
{
    [SerializeField] ObjectTransform[] _object;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] LayerMask layerMask;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int i = 0; i < _object.Length - 1; i++)
            {
                _object[i].DesactivateGraphics();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraHolder.transform.position, cameraHolder.transform.TransformDirection(Vector3.forward), out hit, 8, layerMask))
            {
                Debug.DrawRay(cameraHolder.transform.position, cameraHolder.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 10);
                if (ValidTag(hit.collider.tag))
                {
                    ChangeObject(hit.collider.tag);
                }
            }
        }
    }

    public void ChangeObject(string objectName)
    {
        for (int i = 0; i < _object.Length; i++)
        {
            if (_object[i].objectName == objectName)
            {
                _object[i].Open();
            }
            else if (_object[i].open)
            {
                _object[i].Close();
            }
        }

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("objectTransform", objectName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            ChangeObject((string) changedProps["objectTransform"]);
        }
        //HideSeek.Instance.DisplayWin((int) changedProps["winner"]);
    }

    public bool ValidTag(string tag)
    {
        foreach (var obj in _object)
        {
            if (obj.objectName == tag)
                return true;
        }
        return false;
    }
}
