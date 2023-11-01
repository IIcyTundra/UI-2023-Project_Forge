using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    public float RateOfFire; //rounds per second
    public int ProjectilesPerMag;
    public float reloadTime = 0.3f;
    public float WeaponRange;

    [Header("Projectile")]
    public GameObject BulletPrefab;

    [Header("Effects")]
    //public Transform shell;
    //public Transform shellEjection;
    public AudioClip[] ShootAudio;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

}
