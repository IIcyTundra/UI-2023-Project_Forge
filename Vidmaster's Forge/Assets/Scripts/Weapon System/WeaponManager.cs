using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponHold;
    [SerializeField] private WeaponMechanics[] Guns;
    WeaponMechanics equippedGun;

    public void Start()
    {
        if (Guns[0] != null)
            EquipGun(0);
    }

    public void Update()
    {
        // This stuff should be in the player class caliing the methods of the same name in this class
        if (Input.GetMouseButton(0))
        {
            OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTriggerReleased();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void EquipGun(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex <= Guns.Length)
            EquipGun(Guns[weaponIndex]);
    }

    public void EquipGun(WeaponMechanics gunToEquip)
    {
        if (equippedGun != null)
            Destroy(equippedGun.gameObject);

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerReleased()
    {
        if (equippedGun != null) 
            equippedGun.OnTriggerReleased();
    }

    public void Reload()
    {
        if (equippedGun != null)
            equippedGun.Reload();
    }
}
