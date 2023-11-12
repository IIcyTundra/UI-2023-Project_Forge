using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


public enum AmmoType
{
    Light,
    Heavy,
    Special
}

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private BoxCollider pickupBox;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammoAmmount = 10;

    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float floatHeight = 0.5f;

    private Vector3 initialPosition;
    
    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        

        Vector3 newPosition = initialPosition;
        newPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        transform.position = newPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision with {other.tag}");
        if (other.CompareTag("Player")){
            // This is where to grab the ammo manager from the player
            AmmoManager playerAmmoManager = other.GetComponentInChildren<AmmoManager>();


            if(playerAmmoManager != null)
            {
                // Add the respective ammo and quantity
                playerAmmoManager.AddAmmo(ammoType, ammoAmmount);

                // Destroy self to make the pickup disappear
                Destroy(gameObject);

            } 
        }
    }
}
