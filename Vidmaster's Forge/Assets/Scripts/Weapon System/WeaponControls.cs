using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponControls : MonoBehaviour
{
    public static Action ShootingHeld;
    public static Action ShootingReleased;



    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            ShootingHeld?.Invoke();
            //Debug.Log("Shooting");
        }
    }
}
