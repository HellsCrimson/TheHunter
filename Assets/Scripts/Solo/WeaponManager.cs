using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponData primaryWeapon;

    private WeaponData currentWeapon;
    private WeaponGraphics currentGraphics;
    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public void EquipWeapon(WeaponData _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (currentGraphics == null)
            Debug.LogError("Pas de script WeaponGraphics sur l'arme : " + weaponIns.name);

    }
}
