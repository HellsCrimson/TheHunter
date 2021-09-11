using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagazineUI : MonoBehaviour
{
    public static MagazineUI Instance; 
    
    [SerializeField] private TMP_Text WeaponMagazineUI;

    [SerializeField] public int curMagazine; // munition chargeur actuel
    [SerializeField] public int totalMagazine; // nb chargeur actuel
    public int maxMagazine; // max munition dans un chargeur (reference)
    private int ammoInMag = 30; // munition dans un chargeur (reference)

    void Awake()
    {
        Instance = this;
    }
    
    public void Start()
    {
        curMagazine = ammoInMag;
        totalMagazine = maxMagazine;
        WeaponMagazineUI.text = curMagazine + " / " + totalMagazine;
    }

    void Update()
    {
        ValidMagazine();
        WeaponMagazineUI.text = curMagazine + " / " + totalMagazine;
        
        if (Input.GetKeyDown(KeyCode.R))
            Reload();

        if (totalMagazine == 0 && curMagazine == 0)
            WeaponMagazineUI.text = "Out of ammo";
    }

    private void ValidMagazine()
    {
        if (totalMagazine > maxMagazine)
            totalMagazine = maxMagazine;

        if (curMagazine > ammoInMag)
            curMagazine = ammoInMag;

        if (curMagazine < 0)
            curMagazine = 0;

        if (totalMagazine < 0)
            totalMagazine = 0;
    }

    public void Reload()
    {
        AudioManager.Instance.PlaySound("Reload");
        if (curMagazine != ammoInMag && totalMagazine != 0)
        {
            totalMagazine --;
            curMagazine = ammoInMag;
        }
    }

    public void Shoot()
    {
        curMagazine--;
    }

    public int GetCurAmo()
    {
        return curMagazine;
    }
}
