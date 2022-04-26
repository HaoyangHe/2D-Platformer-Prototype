using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;

    public PlayerWallGrabState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        holdPosition = player.transform.position;
        HoldPosition();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            HoldPosition();
            
            if (grabInput && yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if (!grabInput || yInput < 0)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }

    private void HoldPosition()
    {
        movementAPI.SetVelocityZero();            // Keeps the cinemachine camera focus on the player
        player.transform.position = holdPosition;
    }
}
