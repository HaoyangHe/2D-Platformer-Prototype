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
        
        core.Movement.SetVelocity(playerData.wallHopVelocity, playerData.wallHopAngle, -core.Movement.FacingDirection);

        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("xVelocity", Mathf.Abs(core.Movement.CurrentVelocity.x));
        player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);

        if (Time.time >= startTime + playerData.wallHopTime)
        {
            isAbilityDone = true;
        }
    }
}
