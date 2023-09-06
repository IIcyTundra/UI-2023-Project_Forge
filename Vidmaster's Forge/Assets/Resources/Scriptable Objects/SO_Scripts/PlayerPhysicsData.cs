using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicsData", menuName = "PlayerData/PlayerPhysicsData")]
public class PlayerPhysicsData : ScriptableObject
{
    [Header("Movement")]
    public float Friction;
    public float Gravity;
    public float JumpForce;
    public MovementSettings GroundSettings;
    public MovementSettings AirSettings;
    public MovementSettings StrafeSettings;
}
