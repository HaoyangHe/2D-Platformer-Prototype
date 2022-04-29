using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    // Player Inputs
    private int xInput;
    
    // Collision Senses
    private bool isTouchingWall;

    private int wallJumpDirection;
    
    public PlayerWallJumpState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        xInput = player.InputHandler.NormInputX;

        if ((xInput * movementAPI.FacingDirection < 0 && isTouchingWall) || !isTouchingWall)
        {
            DetermineWallJumpDirection(isTouchingWall);
            movementAPI.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
            movementAPI.CheckIfShouldFlip(wallJumpDirection);
        }
        else if (movementAPI.CurrentVelocity.y <= 0)
        {
            movementAPI.SetVelocityY(playerData.wallJumpVerticalVelocity);
        }

        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();

        isAbilityDone = true;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingWall = collisionSenser.IsTouchingWallFront;
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -movementAPI.FacingDirection;
        }
        else 
        {
            wallJumpDirection = movementAPI.FacingDirection;
        }
    }
}

