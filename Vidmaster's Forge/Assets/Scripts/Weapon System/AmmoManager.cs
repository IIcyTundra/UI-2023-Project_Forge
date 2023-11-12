using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WeaponManager weaponManager;

    [SerializeField] int lightAmmoCount = 100, heavyAmmoCount = 50, specialAmmoCount = 10;

    private Dictionary<AmmoType, int> ammoInventory = new Dictionary<AmmoType, int>();
    
    void Start()
    {
        ammoInventory[AmmoType.Light] = lightAmmoCount;
        ammoInventory[AmmoType.Heavy] = heavyAmmoCount;
        ammoInventory[AmmoType.Special] = specialAmmoCount;

    }

    // Add x ammo for specified type
    public void AddAmmo(AmmoType ammoType, int amount)
    {
        if(ammoInventory.ContainsKey(ammoType))
        {
            ammoInventory[ammoType] += amount;

            Debug.Log($"Added {amount} {ammoType} ammo. Total {ammoInventory[ammoType]} {ammoType} ammo.");

            // Need to push an update to the counter for this.
            weaponManager.OnAmmoPickup();
        }
    }

    // For use in reloading
    public void RemoveAmmo(AmmoType ammoType, int amount)
    {
        if(ammoInventory.ContainsKey(ammoType))
        {
            ammoInventory[ammoType] -= amount;

            Debug.Log($"Removed {amount} {ammoType} ammo. Total {ammoInventory[ammoType]} {ammoType} ammo.");
        }
    }

    // Get ammoCount for specified ammo
    public int GetAmmoCount(AmmoType ammoType)
    {
        return ammoInventory.ContainsKey(ammoType) ? ammoInventory[ammoType] : 0;
    }

}
