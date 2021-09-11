using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootSolo : MonoBehaviour
{
    GameObject audioManager;
    public WeaponData weapon;
    private WeaponManager _weaponManager;

    [SerializeField] private Camera fpscam;

    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager");
        _weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        weapon = _weaponManager.GetCurrentWeapon();
        if (!PauseMenuSolo.GameIsPaused && !GetComponent<HealthBar>().dead)
        {
            if (weapon.fireRate <= 0f && Input.GetButtonDown("Fire1") && MagazineUI.Instance.GetCurAmo() != 0)
                Shoot();
            else
            {
                if (Input.GetButtonDown("Fire1") && MagazineUI.Instance.GetCurAmo() != 0)
                    InvokeRepeating("Shoot", 0f, 1f / weapon.fireRate);
                else if(Input.GetButtonUp("Fire1") || MagazineUI.Instance.GetCurAmo() == 0)
                    CancelInvoke("Shoot");
            }
        }
    }

    public void Shoot()
    {
        audioManager.GetComponent<AudioManager>().PlaySound("ShootingAr");

        GetComponent<WeaponGraphics>().muzzleFlash.Play();
        MagazineUI.Instance.Shoot();

        RaycastHit hit;

        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, weapon.range, layerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("zombie"))
            {
                hit.collider.gameObject.GetComponent<ZombieManager>().curHealth -= weapon.damage;
                HitEffectBlood(hit.point, hit.normal);
            }
            else
            {
                HitEffect(hit.point, hit.normal);
            }
        }
    }

    public void HitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(GetComponent<WeaponGraphics>().hitEffectPrefab,
            pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect,2f);
    }
    
    public void HitEffectBlood(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(GetComponent<WeaponGraphics>().bloodEffect,
            pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect,0.2f);
    }
}
