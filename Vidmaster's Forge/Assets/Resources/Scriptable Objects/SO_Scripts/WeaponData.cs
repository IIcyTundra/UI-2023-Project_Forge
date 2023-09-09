using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon System/Weapon Data"  )]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string GunName;
    public int GunID;

    [Header("Gun Stats")]
    public float GunDamage;
    public float MaxDistance;
    public int MaxAmmoSize;
    public float FireRate;
    public Projectile GunProjectile;

}
