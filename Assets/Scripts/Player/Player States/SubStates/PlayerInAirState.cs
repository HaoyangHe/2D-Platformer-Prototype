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

    private bool hasDrag;
    private bool isAbleToMove;
    private bool xVelocityDrag;
    private bool yVelocityDrag;
    
    public PlayerInAirState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasDrag = false;
        isAbleToMove = false;
        xVelocityDrag = true;
        yVelocityDrag = false;

        if (stateMachine.LastState is PlayerJumpState)
        {
            isJumping = true;
        }
        else if (stateMachine.LastState is PlayerGroundedState)
        {
            coyoteTime = true;
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

        if (isGrounded && movementAPI.CurrentVelocity.y < 0.01f) 
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded) 
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWallBack || (isTouchingWall && xInput != movementAPI.FacingDirection)))    
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
        else if (isTouchingWall && movementAPI.CurrentVelocity.y < -0.01f) 
        {
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

            player.Anim.SetFloat("xVelocity", Mathf.Abs(movementAPI.CurrentVelocity.x));
            player.Anim.SetFloat("yVelocity", movementAPI.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isAbleToMove)
        {
            bool hasFlip = movementAPI.CheckIfShouldFlip(xInput);
            
            if (xInput != 0)
            {
                Vector2 forceToAdd = new Vector2(xInput * playerData.airMovementForce, 0.0f);
                if (hasFlip)
                {
                    if (Mathf.Abs(movementAPI.CurrentVelocity.x) > playerData.movementVelocity)
                    {
                        movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.highSpeedFlipXMultiplier);
                    }
                    else
                    {
                        movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.lowSpeedFlipXMultiplier);
                    }
                }
                movementAPI.AddForce(forceToAdd);
            }

            if (xVelocityDrag)
            {
                if (Mathf.Abs(movementAPI.CurrentVelocity.x) > playerData.movementVelocity)
                {
                    movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.highSpeedXMultiplier);
                }
                else if (xInput == 0 && !hasDrag)
                {
                    hasDrag = true;
                    movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.lowSpeedXMultiplier);
                }
            }

            if (yVelocityDrag && movementAPI.CurrentVelocity.y > playerData.startDargYVelocity)
            {
                movementAPI.SetVelocityY(movementAPI.CurrentVelocity.y * playerData.airDragYMultiplier);
            }
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenser.IsGrounded;
        isTouchingWall = collisionSenser.IsTouchingWallFront;
        isTouchingWallBack = collisionSenser.IsTouchingWallBack;
        isTouchingLedge = collisionSenser.IsTouchingLedge;
        isNearBashAble = collisionSenser.CheckIfNearBashAble();

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

    public void CloseAirDrag()
    {
        hasDrag = true;
    }
}
