using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private float LaunchForce;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject?.GetComponentInParent<Rigidbody>() != null)
        {
            collision.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse); // You can adjust the force as needed
        }
        

        Debug.Log(collision.gameObject?.GetComponentInParent<Rigidbody>().name);
    }
}
