using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponMechanics : MonoBehaviour
{
    [SerializeField] private WeaponData m_WeaponData;
    [SerializeField] private Transform[] WeaponMuzzles;
    [SerializeField] private Camera m_PlayerCam;

    private float timeSinceLastShot;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;


    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    int projectilesRemainingInMag;

    public bool isShooting;

    AudioSource source;

    private void OnEnable()
    {
        WeaponControls.ShootingHeld += Shoot;

        Debug.Log($"enabled" + gameObject.name);
    }

    private void OnDisable()
    {
        WeaponControls.ShootingHeld -= Shoot;

        Debug.Log($"Disabled" + gameObject.name);
    }


    private void Start()
    {
        shotsRemainingInBurst = m_WeaponData.burstCount;
        projectilesRemainingInMag = m_WeaponData.ProjectilesPerMag;
        source = GetComponentInParent<AudioSource>();
    }

    private void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, m_WeaponData.recoilMoveSettleTime);

        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, m_WeaponData.recoilRotationSettleTime);
        transform.localEulerAngles = Vector3.left * recoilAngle;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (m_WeaponData.RateOfFire / 60f);

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }


    public void Shoot()
    {
        int i = Random.Range(0, m_WeaponData.ShootAudio.Length);
        if (projectilesRemainingInMag > 0)
        { 
            if (CanShoot())
            {
                switch (m_WeaponData.fireMode)
                {
                    case FireMode.Burst:
                        FireBurst();
                        break;
                    case FireMode.Single:
                        if (!triggerReleasedSinceLastShot) return;
                        ChooseBulletType();
                        break;
                    case FireMode.Automatic:
                        ChooseBulletType();
                        break;
                }



                // Initiate Recoil
                transform.localPosition -= Vector3.forward * Random.Range(m_WeaponData.kickMinMax.x, m_WeaponData.kickMinMax.y);
                recoilAngle += Random.Range(m_WeaponData.recoilAngleMinMax.x, m_WeaponData.recoilAngleMinMax.y);
                recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);


                source.PlayOneShot(m_WeaponData.ShootAudio?[i]);
            }
        }


    }

    private void ChooseBulletType()
    {
        switch (m_WeaponData.bulletType)
        {
            case BulletType.hitscan:
                FireRaycast();
                break;
            case BulletType.projectile:
                SpawnProjectile();
                break;
        }
    }


    IEnumerator FireBurst()
    {

        for (int i = 0; i < m_WeaponData.burstCount; i++)
        {
            ChooseBulletType();
            yield return new WaitForSeconds(1f / m_WeaponData.RateOfFire);
        }

    }
    private void FireRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_PlayerCam.transform.position, m_PlayerCam.transform.forward, out hit, m_WeaponData.WeaponRange))
        {
            Debug.Log(hit.transform.name);
        }
        shotsRemainingInBurst--;
    }


    private void SpawnProjectile()
    {
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
    }

}

