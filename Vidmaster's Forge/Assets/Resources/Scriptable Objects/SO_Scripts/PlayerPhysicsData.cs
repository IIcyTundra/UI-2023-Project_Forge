using Hertzole.ScriptableValues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicsData", menuName = "PlayerData/PlayerPhysicsData")]
public class PlayerPhysicsData : ScriptableObject
{
    [Header("Movement")]
    public ScriptableFloat Friction;
    public ScriptableFloat Gravity;
    public ScriptableFloat JumpForce;
    public MovementSettings GroundSettings;
    public MovementSettings AirSettings;
    public MovementSettings StrafeSettings;


}
