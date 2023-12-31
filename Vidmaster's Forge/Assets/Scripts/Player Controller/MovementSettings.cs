using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementSettings
{
    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;

    public MovementSettings(float maxSpeed, float accel, float decel)
    {
        MaxSpeed = maxSpeed;
        Acceleration = accel;
        Deceleration = decel;
    }
}
