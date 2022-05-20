using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpStateDouble : PlayerAbilityState
{
    public PlayerJumpStateDouble(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        movementAPI.SetVelocityY(playerData.jumpVelocityNo2);
        isAbilityDone = true;
    }
}
