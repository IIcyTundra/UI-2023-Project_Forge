using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
