using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    private WeaponMechanics WeaponVars;

    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private void Start()
    {

        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            weapons[i] = transform.GetChild(i);

        if (keys == null) keys = new KeyCode[weapons.Length];
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keys.Length; i++)
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
                selectedWeapon = i;


        ScrollSelect();

        if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);

        timeSinceLastSwitch += Time.deltaTime;

        

    }

    private void ScrollSelect()
    {
        // Detect scroll input to change weapons
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= weapons.Length - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = weapons.Length - 1;
            else
                selectedWeapon--;
        }
    }
    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].gameObject.SetActive(i == weaponIndex);

        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }
    private void OnWeaponSelected() 
    {
        // Need to update the ammo counter upon switching the weapon, every time
        WeaponMechanics selectedWeaponMechanics = weapons[selectedWeapon].GetComponent<WeaponMechanics>();
        selectedWeaponMechanics.UpdateAmmoCounter();
    }

    public void OnAmmoPickup()
    {
        WeaponMechanics selectedWeaponMechanics = weapons[selectedWeapon].GetComponent<WeaponMechanics>();
        selectedWeaponMechanics.UpdateAmmoCounter();
    }
}
