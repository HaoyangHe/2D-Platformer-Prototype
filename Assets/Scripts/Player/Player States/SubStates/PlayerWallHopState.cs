using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallHopState : PlayerAbilityState
{
    public PlayerWallHopState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        
        player.SetVelocity(playerData.wallHopVelocity, playerData.wallHopAngle, -player.FacingDirection);

        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);

        if (Time.time >= startTime + playerData.wallHopTime)
        {
            isAbilityDone = true;
        }
    }
}
