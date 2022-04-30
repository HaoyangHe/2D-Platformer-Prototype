using UnityEngine;

public class PlayerWallJumpVerticalState : PlayerAbilityState
{
    // Collision Senses
    private bool isTouchingLedge;

    public PlayerWallJumpVerticalState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();

        movementAPI.SetVelocityY(playerData.wallJumpUpwardVelocity);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isTouchingLedge || movementAPI.CurrentVelocity.y <= 0)
        { 
            isAbilityDone = true;
        }

        movementAPI.GravityScale(playerData.wallJumpVerticalGravitiScale);

        player.Anim.SetFloat("xVelocity", Mathf.Abs(movementAPI.CurrentVelocity.x) >= 0.15f ? 1.0f : 0.0f);
        player.Anim.SetFloat("yVelocity", movementAPI.CurrentVelocity.y);
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingLedge = collisionSenser.IsTouchingLedge;
    }
}
