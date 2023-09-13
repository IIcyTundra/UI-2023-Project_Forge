using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("Aiming")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private MouseLook m_MouseLook = new MouseLook();

    [SerializeField] private PlayerPhysicsData m_PlayerPhysicsData;
    [Tooltip("Automatically jump when holding jump button")]
    [SerializeField] private bool m_AutoBunnyHop = false;
    [Tooltip("How precise air control is")]
    [SerializeField] private float m_AirControl = 0.3f;
    [Tooltip("The Maximum Angle that you can move up a slope")]
    [SerializeField] private float m_MaxSlopeAngle;

    /// <summary>
    /// Returns player's current speed.
    /// </summary>
    public float Speed { get { return m_Character.velocity.magnitude; } }

    private CharacterController m_Character;
    private Vector3 m_MoveDirectionNorm = Vector3.zero;
    private Vector3 m_PlayerVelocity = Vector3.zero;

    // Used to queue the next jump just before hitting the ground.
    private bool m_JumpQueued = false;

    // Used to display real time friction values.
    private float m_PlayerFriction = 0;

    //Used to detect for slopes
    private RaycastHit m_slopeHit;

    private Vector3 m_MoveInput;
    private Transform m_Tran;
    private Transform m_CamTran;

    private void Start()
    {
        m_Tran = transform;
        m_Character = GetComponent<CharacterController>();

        if (!m_Camera)
            m_Camera = Camera.main;

        m_CamTran = m_Camera.transform;
        m_MouseLook.Init(m_Tran, m_CamTran);
    }

    private void Update()
    {
        m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        m_MouseLook.UpdateCursorLock();
        QueueJump();

        // Set movement state.
        if (m_Character.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove();
        }

        // Rotate the character and camera.
        m_MouseLook.LookRotation(m_Tran, m_CamTran);

        // Move the character.
        m_Character.Move(m_PlayerVelocity * Time.deltaTime);
    }

    // Queues the next jump.
    private void QueueJump()
    {
        if (m_AutoBunnyHop)
        {
            m_JumpQueued = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !m_JumpQueued)
        {
            m_JumpQueued = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            m_JumpQueued = false;
        }
    }

    // Handle air movement.
    private void AirMove()
    {
        float accel;

        var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        wishdir = m_Tran.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= m_PlayerPhysicsData.AirSettings.MaxSpeed;

        wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        // CPM Air control.
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
        {
            accel = m_PlayerPhysicsData.AirSettings.Deceleration;
        }
        else
        {
            accel = m_PlayerPhysicsData.AirSettings.Acceleration;
        }

        // If the player is ONLY strafing left or right
        if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
        {
            if (wishspeed > m_PlayerPhysicsData.StrafeSettings.MaxSpeed)
            {
                wishspeed = m_PlayerPhysicsData.StrafeSettings.MaxSpeed;
            }

            accel = m_PlayerPhysicsData.StrafeSettings.Acceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (m_AirControl > 0)
        {
            AirControl(wishdir, wishspeed2);
        }

        // Apply gravity
        m_PlayerVelocity.y -= m_PlayerPhysicsData.Gravity * Time.deltaTime;
    }

    // Air control occurs when the player is in the air, it allows players to move side 
    // to side much faster rather than being 'sluggish' when it comes to cornering.
    private void AirControl(Vector3 targetDir, float targetSpeed)
    {
        // Only control air movement when moving forward or backward.
        if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
        {
            return;
        }

        float zSpeed = m_PlayerVelocity.y;
        m_PlayerVelocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        float speed = m_PlayerVelocity.magnitude;
        m_PlayerVelocity.Normalize();

        float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
        float k = 32;
        k *= m_AirControl * dot * dot * Time.deltaTime;

        // Change direction while slowing down.
        if (dot > 0)
        {
            m_PlayerVelocity.x *= speed + targetDir.x * k;
            m_PlayerVelocity.y *= speed + targetDir.y * k;
            m_PlayerVelocity.z *= speed + targetDir.z * k;

            m_PlayerVelocity.Normalize();
            m_MoveDirectionNorm = m_PlayerVelocity;
        }

        m_PlayerVelocity.x *= speed;
        m_PlayerVelocity.y = zSpeed; // Note this line
        m_PlayerVelocity.z *= speed;
    }

    // Handle ground movement.
    private void GroundMove()
    {
        // Do not apply friction if the player is queueing up the next jump
        if (!m_JumpQueued)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0);
        }

        var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        wishdir = m_Tran.TransformDirection(wishdir);
        wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= m_PlayerPhysicsData.GroundSettings.MaxSpeed;

        Accelerate(wishdir, wishspeed, m_PlayerPhysicsData.GroundSettings.Acceleration);

        // Reset the gravity velocity
        m_PlayerVelocity.y = -m_PlayerPhysicsData.Gravity * Time.deltaTime;
        m_PlayerVelocity = IsOnSlope(m_PlayerVelocity);

        if (m_JumpQueued)
        {
            m_PlayerVelocity.y = m_PlayerPhysicsData.JumpForce;
            m_JumpQueued = false;
        }

        
    }

    private Vector3 IsOnSlope(Vector3 velocity)
    {
        if(Physics.Raycast(transform.position, Vector3.down, out m_slopeHit, m_Character.height * 0.5f + 0.3f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, m_slopeHit.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0) 
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }



    private void ApplyFriction(float t)
    {
        // Equivalent to VectorCopy();
        Vector3 vec = m_PlayerVelocity;
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        // Only apply friction when grounded.
        if (m_Character.isGrounded)
        {
            float control = speed < m_PlayerPhysicsData.GroundSettings.Deceleration ? m_PlayerPhysicsData.GroundSettings.Deceleration : speed;
            drop = control * m_PlayerPhysicsData.Friction * Time.deltaTime * t;
        }

        float newSpeed = speed - drop;
        m_PlayerFriction = newSpeed;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (speed > 0)
        {
            newSpeed /= speed;
        }

        m_PlayerVelocity.x *= newSpeed;
        // playerVelocity.y *= newSpeed;
        m_PlayerVelocity.z *= newSpeed;
    }

    // Calculates acceleration based on desired speed and direction.
    private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
    {
        float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
        float addspeed = targetSpeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }

        float accelspeed = accel * Time.deltaTime * targetSpeed;
        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        m_PlayerVelocity.x += accelspeed * targetDir.x;
        m_PlayerVelocity.z += accelspeed * targetDir.z;
    }

}
