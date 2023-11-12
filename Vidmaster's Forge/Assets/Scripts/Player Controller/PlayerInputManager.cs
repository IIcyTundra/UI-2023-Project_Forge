using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerInputReader")]
public class PlayerInputManager : ScriptableObject, PlayerInput.IPlayerActionMapActions
{
    [SerializeField] private PlayerStateMachine m_StateMachine;
    private PlayerInput m_PlayerInput;

    private void OnEnable()
    {
        if(m_PlayerInput == null)
        {
            m_PlayerInput = new PlayerInput();

            m_PlayerInput.PlayerActionMap.SetCallbacks(this);
        }
    }

    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_PlayerInput.PlayerActionMap.Jump.performed += context =>
        {
            m_StateMachine.IsJumpPressed = context.ReadValue<bool>();
        };
        
    }

    public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_PlayerInput.PlayerActionMap.Look.performed += context =>
        {
            //m_StateMachine.m_MouseLook.xRot = context.ReadValue<Vector2>().x;
            //m_StateMachine.m_MouseLook.yRot = context.ReadValue<Vector2>().y;
        };

    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_PlayerInput.PlayerActionMap.Move.performed += context =>
        {
            m_StateMachine.m_MoveInput = context.ReadValue<Vector2>();
        };
    }

    public void OnPrimaryShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //BLAM
    }

    public void OnSecondaryShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //BLAM
    }

    public void OnReload(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Reloading...
    }
}
