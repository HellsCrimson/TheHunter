using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string name = "Fn-Scar";
    public float damage = 10f;
    public float range = 200f;

    public float fireRate = 10f;

    public GameObject graphics;
}
