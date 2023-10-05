using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    idle,
    grounded,
    air,
    walk,
    shooting,
    jumping
}

public class PlayerStateMachine : MonoBehaviour
{
    #region In Editor Variables

    [Header("Input Detection")]
    [SerializeField] private PlayerInputManager m_PlayerInput;


    [Header("Aiming")]
    [SerializeField] private Camera m_Camera;
    public MouseLook m_MouseLook = new MouseLook();

    [Header("Player Physics")]
    [SerializeField] private PlayerPhysicsData m_PlayerPhysicsData;
    [Tooltip("Automatically jump when holding jump button")]
    [SerializeField] private bool m_AutoBunnyHop = false;
    [Tooltip("How precise air control is")]
    [SerializeField] private float m_AirControl = 0.3f;
    [Tooltip("The Maximum Angle that you can move up a slope")]
    [SerializeField] private float m_MaxSlopeAngle;

    #endregion

    #region Other Variables
    public static PlayerStateMachine Instance;

    //Getters & Setters
    //public PlayerBaseState CurrentState { get { return m_CurrentState; } set { m_CurrentState = value; } } //returns player state
    public float Speed { get { return m_Character.velocity.magnitude; } } //returns player speed
    public bool IsJumpPressed { get; set; }




    private CharacterController m_Character;
    private Vector3 m_MoveDirectionNorm = Vector3.zero;
    private Vector3 m_PlayerVelocity = Vector3.zero;

    // Used to queue the next jump just before hitting the ground.
    private bool m_JumpQueued = false;

    // Used to display real time friction values.
    private float m_PlayerFriction = 0;

    //Used to detect for slopes
    private RaycastHit m_slopeHit;

    [HideInInspector] public Vector3 m_MoveInput;
    private Transform m_Tran;
    private Transform m_CamTran;

    #endregion

    private void Awake()
    {
        //m_States = new PlayerStateFactory(this);
        //m_CurrentState = m_States.Grounded();
        //m_CurrentState.EnterState();
        m_Tran = transform;
        m_Character = GetComponent<CharacterController>();

        if (!m_Camera)
            m_Camera = Camera.main;

        m_CamTran = m_Camera.transform;
        m_MouseLook.Init(m_Tran, m_CamTran);
    }

    private void Start()
    {
        
    }

   
}
