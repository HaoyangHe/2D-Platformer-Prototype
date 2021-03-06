using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            movementAPI.GravityScale(playerData.wallSlideAcceleration);

            if (movementAPI.CurrentVelocity.y < -playerData.wallSlideVelocity)
            {
                movementAPI.SetVelocityY(-playerData.wallSlideVelocity);
            }

            if (grabInput && yInput == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (grabInput && yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
        }
    }
}
