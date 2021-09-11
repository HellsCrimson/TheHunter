using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerShoot : MonoBehaviourPun
{
    static PlayerShoot Instance;
    
    PlayerController controller;
    
    [SerializeField] private bool IsHunter;
    [SerializeField] private Camera fpscam;
    [SerializeField] private LayerMask layerMask;
    
    PhotonView PV;
    
    public int gunDamage = 20;
    public float range = 100f;
    public float fireRate = 0.05f;
    private float nextFire;

    void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!PV.IsMine || !IsHunter)
            return;
        if (!PauseMenuMulti.GameIsPaused && Input.GetButton("Fire1") &&
            Time.time > nextFire && MagazineUI.Instance.GetCurAmo() != 0)
            Shoot();
    }

    public void Shoot()
    {
        GetComponent<WeaponGraphics>().muzzleFlash.Play();
        MagazineUI.Instance.Shoot();
        nextFire = Time.time + fireRate;
        AudioManager.Instance.PlaySound("LaserGun");

        RaycastHit hit;

        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range, layerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("player"))
            {
                hit.collider.GetComponentInParent<PlayerController>().TakeDamage(gunDamage);
                PV.RPC("HitEffectBlood", RpcTarget.All, hit.point, hit.normal);
                
            }
            else
            {
                PV.RPC("HitEffect", RpcTarget.All, hit.point, hit.normal);
            }
        }
    }

    [PunRPC]
    public void HitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(GetComponent<WeaponGraphics>().hitEffectPrefab,
            pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect,2f);
    }
    
    [PunRPC]
    public void HitEffectBlood(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(GetComponent<WeaponGraphics>().bloodEffect,
            pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect,0.2f);
    }
}
