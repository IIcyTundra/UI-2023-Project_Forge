using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMechanics : MonoBehaviour
{
    [SerializeField] private WeaponData m_WeaponData;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;


    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    int projectilesRemainingInMag;

    bool isReloading;

    AudioSource source;

    private void Start()
    {
        shotsRemainingInBurst = m_WeaponData.burstCount;
        projectilesRemainingInMag = m_WeaponData.ProjectilesPerMag;
        source = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, m_WeaponData.recoilMoveSettleTime);


        if (!isReloading)
        {
            recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, m_WeaponData.recoilRotationSettleTime);
            transform.localEulerAngles = Vector3.left * recoilAngle;
            if (projectilesRemainingInMag == 0)
                Reload();
        }
    }

    private void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
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


            nextShotTime = Time.time + m_WeaponData.msBetweenShots / 1000f;
            // Spawn projectiles
            foreach (Transform spawn in m_WeaponData.ProjectileSpawns)
            {
                if (projectilesRemainingInMag == 0) break;
                projectilesRemainingInMag--;
                
            }

            // Eject shell
            Instantiate(m_WeaponData.shell, m_WeaponData.shellEjection.position, m_WeaponData.shellEjection.rotation);

            // Initiate Recoil
            transform.localPosition -= Vector3.forward * Random.Range(m_WeaponData.kickMinMax.x, m_WeaponData.kickMinMax.y);
            recoilAngle += Random.Range(m_WeaponData.recoilAngleMinMax.x, m_WeaponData.recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            source.PlayOneShot(m_WeaponData.shootAudio, 1);
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != m_WeaponData.ProjectilesPerMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        source.PlayOneShot(m_WeaponData.reloadAudio, 1);

        float reloadSpeed = 1 / m_WeaponData.reloadTime;
        float percent = 0;

        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30.0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = m_WeaponData.ProjectilesPerMag;
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerReleased()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = m_WeaponData.burstCount;
    }
}

