using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // Player Inputs
    private int xInput;
    private bool jumpInput;
    private bool grabInput;
    private bool bashInput;
    private bool jumpInputStop;

    // Collision Senses
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingLedge;
    private bool isNearBashAble;
    
    private bool coyoteTime;
    private bool isJumping;

    // Movement States Triggers
    private bool canApplyMovement;
    private bool canApplyXDrag;
    private bool canApplyBashClamp;

    public PlayerInAirState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canApplyMovement = true;
        canApplyBashClamp = false;

        if ((stateMachine.LastState is PlayerJumpState || 
            stateMachine.LastState is PlayerWallJumpHorizontalState) && 
            player.JumpState.amountOfJumpsLeft == playerData.amountOfJumps - 1)
        {
            isJumping = true;
            canApplyXDrag = true;
        }
        else if (stateMachine.LastState is PlayerGroundedState)
        {
            coyoteTime = true;
            canApplyXDrag = true;
        }
        else if (stateMachine.LastState is PlayerBashState)
        {
            canApplyMovement = false;
            canApplyXDrag = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        coyoteTime = false;
        isJumping = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        bashInput = player.InputHandler.BashInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        CheckCoyoteTime();          
        CheckJumpMultiplier();

        if (isGrounded && xInput != 0 && movementAPI.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isGrounded && movementAPI.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded && !isTouchingWallBack)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && xInput * movementAPI.FacingDirection > 0 && isTouchingWall)
        {
            stateMachine.ChangeState(player.WallJumpVerticalState);
        }
        else if (jumpInput && (isTouchingWallBack || (isTouchingWall && xInput * movementAPI.FacingDirection < 0)))
        {
            stateMachine.ChangeState(player.WallJumpHorizontalState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && isTouchingLedge && grabInput && !isTouchingWallBack)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && movementAPI.CurrentVelocity.y < -0.01f && !isTouchingWallBack)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (isNearBashAble && bashInput)
        {
            stateMachine.ChangeState(player.BashState);
        }
        else
        {
            if (canApplyMovement)
            {
                if (xInput == 0 && canApplyXDrag)
                {
                    movementAPI.DecreaseVelocityX(Mathf.Sign(movementAPI.CurrentVelocity.x) * 
                                                  playerData.airDragDeceleration * Time.deltaTime);
                }
                else
                {
                    if (movementAPI.CheckIfShouldFlip(xInput))
                    {
                        movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.airFlipMultiplier);
                    }

                    movementAPI.AddVelocityX(xInput * playerData.airMovementAcceleration * Time.deltaTime);

                    if (canApplyBashClamp)
                    {
                        movementAPI.ClampVelocityX(playerData.bashVelocityClamp);
                    }
                    else
                    { 
                        movementAPI.ClampVelocityX(playerData.movementVelocity);
                    }
                }
            }

            if (xInput != 0)
            {
                if (!canApplyMovement && Mathf.Abs(movementAPI.CurrentVelocity.x) >= playerData.movementVelocity) 
                { 
                    canApplyBashClamp = true;
                }

                canApplyMovement = true;
                canApplyXDrag = true;
            }

            if (movementAPI.CurrentVelocity.y > 0)
            {
                if (stateMachine.LastState is PlayerWallJumpHorizontalState)
                {
                    movementAPI.GravityScale(playerData.wallJumpUpwardGravityScale);
                }
                else if (stateMachine.LastState is PlayerBashState)
                {
                    movementAPI.GravityScale(playerData.bashUpwardGravityScale);
                }
                else 
                {
                    movementAPI.GravityScale(playerData.airUpwardGravityScale);
                }

                movementAPI.SetVelocityY(movementAPI.CurrentVelocity.y < 0 ? 0 : movementAPI.CurrentVelocity.y);
            }
            else
            {
                movementAPI.GravityScale(playerData.airFallingGravityScale);
            }

            player.Anim.SetFloat("xVelocity", Mathf.Abs(movementAPI.CurrentVelocity.x) >= 0.15f ? 1.0f : 0.0f);
            player.Anim.SetFloat("yVelocity", movementAPI.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenser.IsGrounded;
        isTouchingWall = collisionSenser.IsTouchingWallFront;
        isTouchingWallBack = collisionSenser.IsTouchingWallBack;
        isTouchingLedge = collisionSenser.IsTouchingLedge;
        isNearBashAble = collisionSenser.IsNearBashAble;

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    private void CheckCoyoteTime()      // Player can still jump after fell from the edge within a very short period of time.
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTimeInAir)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckJumpMultiplier()  // Player can achieve variable jump height by holding the JumpButton for a different period of time.
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                movementAPI.SetVelocityY(playerData.variableJumpHeightMultiplier * movementAPI.CurrentVelocity.y);
                isJumping = false;
            }
            else if (movementAPI.CurrentVelocity.y < -0.01f)
            {
                isJumping = false;
            }
        }
    }
}
