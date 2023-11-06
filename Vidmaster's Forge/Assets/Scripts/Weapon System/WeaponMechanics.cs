using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponMechanics : MonoBehaviour
{
    [SerializeField] private WeaponData m_WeaponData;
    [SerializeField] private Transform[] WeaponMuzzles;

    private float timeBetweenShots;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;


    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    int projectilesRemainingInMag;

    public bool isShooting;

    AudioSource source;

    private void OnEnable()
    {
        WeaponControls.ShootingHeld += OnTriggerHold;
        WeaponControls.ShootingReleased += OnTriggerReleased;

        Debug.Log($"enabled" + gameObject.name);
    }

    private void OnDisable()
    {
        WeaponControls.ShootingHeld -= OnTriggerHold;
        WeaponControls.ShootingReleased -= OnTriggerReleased;

        Debug.Log($"Disabled" + gameObject.name);
    }


    private void Start()
    {
        nextShotTime = Time.time;
        shotsRemainingInBurst = m_WeaponData.burstCount;
        projectilesRemainingInMag = m_WeaponData.ProjectilesPerMag;
        source = GetComponentInParent<AudioSource>();
        timeBetweenShots = 1.0f / m_WeaponData.RateOfFire;
    }

    private void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, m_WeaponData.recoilMoveSettleTime);

        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, m_WeaponData.recoilRotationSettleTime);
        transform.localEulerAngles = Vector3.left * recoilAngle;
    }

    public void Shoot()
    {
        if (Time.time >= nextShotTime && projectilesRemainingInMag > 0)
        {
            // Firemodes
            if (m_WeaponData.fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (m_WeaponData.fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot) return;
            }


            nextShotTime = Time.time + timeBetweenShots;
            // Spawn projectiles
            SpawnBullet();


            // Initiate Recoil
            transform.localPosition -= Vector3.forward * Random.Range(m_WeaponData.kickMinMax.x, m_WeaponData.kickMinMax.y);
            recoilAngle += Random.Range(m_WeaponData.recoilAngleMinMax.x, m_WeaponData.recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);


            int j = Random.Range(0, m_WeaponData.ShootAudio.Length);
            source.PlayOneShot(m_WeaponData.ShootAudio?[j]);
        }
        else
        {
            source.PlayOneShot(m_WeaponData.EmptyMagAudio);
        }


    }




    private void SpawnBullet()
    {
        isShooting = false;
        foreach (Transform muzzle in WeaponMuzzles)
        {
            GameObject bullet = ObjectPools.Instance.GetPooledObject(m_WeaponData.BulletPrefab.name);
            if (bullet.activeSelf != true)

            {
                bullet.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);

                bullet.SetActive(true);

                bullet.transform.forward = muzzle.forward;
                bullet?.GetComponent<Projectile>().AddImpactListener();

                projectilesRemainingInMag--;
            }
        }
        isShooting = true;
    }


    public void OnTriggerHold()
    { 
        if(!isShooting)
        {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }
        
    }

    public void OnTriggerReleased()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = m_WeaponData.burstCount;
        isShooting = false;
    }
}

