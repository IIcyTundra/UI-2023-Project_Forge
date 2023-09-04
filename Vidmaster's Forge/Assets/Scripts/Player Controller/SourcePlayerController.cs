using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class SourcePlayerCOntroller : MonoBehaviour
    {
        [SerializeField] float m_Friction = 4.0f;
        [SerializeField] float m_MaxAccel = 10f;
        [SerializeField] float m_MaxAirAccel = 300f;
        [SerializeField] private bool PlaySounds;
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField][Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier = 1.16147f;
        [SerializeField] private MouseLook m_MouseLook;
        //[SerializeField] private bool m_UseFovKick;
        //[SerializeField] private FOVKick m_FovKick = new FOVKick();
        //[SerializeField] private bool m_UseHeadBob;
        //[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        //[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.



        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        bool m_HasJumped = false; // False if on ground, true if not on ground and has jumped
        private AudioSource m_AudioSource;
        float m_MaxSpeed;


        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = GetComponentInChildren<Camera>();
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            //m_FovKick.Setup(m_Camera);
            //m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            Vector3 velocity = m_CharacterController.velocity;
            GetInput(out m_MaxSpeed);
            float speed = new Vector2(velocity.x, velocity.z).magnitude;

            // Apply half-gravity (leapfrog integration; we'll add the rest below.):
            velocity.y -= (15.24f * 0.5f * Time.fixedDeltaTime);

            // Translate input vector to world-space based on camera:
            Vector3 accelDir = transform.forward * m_Input.y + transform.right * m_Input.x;
            accelDir = new Vector3(accelDir.x, 0.0f, accelDir.z).normalized;

            // Calculate friction:
            Friction(speed, ref velocity);

            // Calculate acceleration:
            var projectedSpeed = Vector3.Dot(velocity, accelDir);
            bool onGround = m_CharacterController.isGrounded;
            var addSpeed = (onGround ? m_MaxSpeed : (3.0 / 40.0) * m_MaxSpeed) - projectedSpeed;
            if (addSpeed < 0)
            {
                addSpeed = 0;
            }
            var accelSpeed =
                            Mathf.Clamp(
                                (onGround ? m_MaxAccel : m_MaxAirAccel) * m_MaxSpeed * Time.fixedDeltaTime,
                                0.0f, (float)addSpeed);

            // Apply acceleration to velocity:
            velocity += accelDir * accelSpeed;

            // Handle jumping
            if (onGround && m_Jump && !m_HasJumped)
            {
                velocity.y += 5.1111f;
                m_HasJumped = true;
            }

            // Apply other half of gravity:
            velocity.y -= (15.24f * 0.5f * Time.fixedDeltaTime);

            // Move based on velocity:
            m_CharacterController.SimpleMove(velocity);

            // Reset hasJumped if we haven't jumped:
            if (onGround)
            {
                m_HasJumped = false;
            }


            ProgressStepCycle(m_MaxSpeed);
            UpdateCameraPosition(m_MaxSpeed);

            m_MouseLook.UpdateCursorLock();
        }

        void Friction(float speed, ref Vector3 velocity)
        {
            var drop = 0.0f;
            if (speed < 0.00191)
                return;
            if (m_CharacterController.isGrounded && !m_Jump)
            {
                double stopSpeed = (5.0 / 16.0) * m_MaxSpeed;
                double control = (speed < stopSpeed) ? stopSpeed : speed;
                drop = (float)(control * m_Friction * Time.fixedDeltaTime);
            }

            float newSpeed = speed - drop;
            if (newSpeed < 0)
                newSpeed = 0;
            newSpeed /= speed;
            velocity *= newSpeed;
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            //PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            //if (!m_UseHeadBob)
            //{
            //    return;
            //}
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                //m_Camera.transform.localPosition =
                //    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                //                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                //newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                //newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            bool waswalking = m_IsWalking;

            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            //if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            //{
            //    StopAllCoroutines();
            //    StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            //}
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //don't move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
