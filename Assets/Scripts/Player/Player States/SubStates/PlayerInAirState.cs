using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool bashInput;

    // Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingLedge;
    private bool isNearBashAble;
    
    private bool coyoteTime;
    private bool isJumping;
    private bool hasDrag;
    private bool isAbleToMove;
    private bool xVelocityDrag;
    private bool yVelocityDrag;
    
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasDrag = false;
        xVelocityDrag = true;
        yVelocityDrag = false;
        isAbleToMove = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        bashInput = player.InputHandler.BashInput;

        CheckJumpMultiplier();

        DoChecks();

        isNearBashAble = player.CheckIfNearBashAble();

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWallBack || (isTouchingWall && xInput != player.FacingDirection)))
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump()) 
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && isTouchingLedge && grabInput)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && player.CurrentVelocity.y < -0.01f)
        {
            player.WallSlideState.StartCoyoteTime();
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (isNearBashAble && bashInput)
        {
            stateMachine.ChangeState(player.BashState);
        }
        else 
        {
            isAbleToMove = true;

            if (player.BashState.WasBash)
            {
                hasDrag = true;
                yVelocityDrag = true;
                player.BashState.ResetWasBash();
            }

            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isAbleToMove)
        {
            bool hasFlip = player.CheckIfShouldFlip(xInput);
            
            if (xInput != 0)
            {
                Vector2 forceToAdd = new Vector2(xInput * playerData.airMovementForce, 0.0f);
                if (hasFlip)
                {
                    if (Mathf.Abs(player.CurrentVelocity.x) > playerData.movementVelocity)
                    {
                        player.SetVelocityX(player.CurrentVelocity.x * playerData.highSpeedFlipXMultiplier);
                    }
                    else
                    {
                        player.SetVelocityX(player.CurrentVelocity.x * playerData.lowSpeedFlipXMultiplier);
                    }
                }
                player.AddForce(forceToAdd);
            }

            if (xVelocityDrag)
            {
                if (Mathf.Abs(player.CurrentVelocity.x) > playerData.movementVelocity)
                {
                    player.SetVelocityX(player.CurrentVelocity.x * playerData.highSpeedXMultiplier);
                }
                else if (xInput == 0 && !hasDrag)
                {
                    hasDrag = true;
                    player.SetVelocityX(player.CurrentVelocity.x * playerData.lowSpeedXMultiplier);
                }
            }

            if (yVelocityDrag && player.CurrentVelocity.y > playerData.startDargYVelocity)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.airDragYMultiplier);
            }
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isNearBashAble = player.CheckIfNearBashAble();

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(playerData.variableJumpHeightMultiplier * player.CurrentVelocity.y);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y < -0.01f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTimeInAir)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() 
    {
        coyoteTime = true;
    }

    public void SetIsJumping()
    {
        isJumping = true;
    }

    public void CloseAirDrag()
    {
        hasDrag = true;
    }
}
