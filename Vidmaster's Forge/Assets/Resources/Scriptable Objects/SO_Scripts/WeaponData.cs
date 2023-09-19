using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FireMode
{
    Single,
    Burst,
    Automatic
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon System/Weapon Data"  )]
public class WeaponData : ScriptableObject
{
    
    [Header("Stats")]
    public float MuzzleVelocity;
    public FireMode fireMode = FireMode.Single;
    public int burstCount;
    public float msBetweenShots = 100;
    public int ProjectilesPerMag;
    public float reloadTime = 0.3f;

    [Header("Projectile")]
    public float ProjectileSpeed;

    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

}
