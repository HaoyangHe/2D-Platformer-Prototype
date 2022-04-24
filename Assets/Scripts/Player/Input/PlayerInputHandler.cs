using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;
    
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawBashDirecionInput { get; private set; }
    
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool BashInput { get; private set; }
    public bool BashInputStop { get; private set; }
    public float XInputStartTime { get; private set; }
    public float YInputStartTime { get; private set; }

    [SerializeField]
    private float xInputTolerance = 0.5f;
    
    [SerializeField]
    private float yInputTolerance = 0.5f;
    
    [SerializeField]
    private float jumpInputHoldTime = 0.2f;

    private float jumpInputStartTime;

    private Transform BashDirectionIndicator;
    
    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;    
        BashDirectionIndicator = transform.Find("BashDirectionIndicator");
    }

    private void Update() 
    {
        CheckJumpInputHoldTime();           
    }

    public void OnMoveInput(InputAction.CallbackContext context) 
    {
        RawMovementInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(RawMovementInput.x) > xInputTolerance)
        {
            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
            XInputStartTime = Time.time;
        }
        else
        {
            NormInputX = 0;
        }

        if (Mathf.Abs(RawMovementInput.y) > yInputTolerance)
        {
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
            YInputStartTime = Time.time;
        }
        else
        {
            NormInputY = 0;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnBashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BashInput = true;
            BashInputStop = false;
        }

        if (context.canceled)
        {
            BashInput = false;
            BashInputStop = true;
        }
    }

    public void OnBashDirectionInput(InputAction.CallbackContext context)
    {
        RawBashDirecionInput = context.ReadValue<Vector2>();
        
        if (playerInput.currentControlScheme == "Keyboard")
        {
            RawBashDirecionInput = RawBashDirecionInput - (Vector2)cam.WorldToScreenPoint(BashDirectionIndicator.position);
        }
    }

    public void UseJumpInput()
    {
        JumpInput = false;
    }

    public void UseBashInput()
    {
        BashInput = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + jumpInputHoldTime)
        {
            JumpInput = false;
        }
    }
}
