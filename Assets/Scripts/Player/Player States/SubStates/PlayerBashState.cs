using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBashState : PlayerAbilityState
{
    public bool WasBash { get; private set; }
    public bool CanBash { get; private set; }
    
    private Vector2 bashDirction;
    private Vector2 bashDirctionInput;
    
    private int xInput;
    private bool isHolding;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool bashInputStop;

    public PlayerBashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName)
    {
        CanBash = true;
    }

    public override void Enter()
    {
        base.Enter();
        
        startTime = Time.unscaledTime;
        Time.timeScale = playerData.holdTimeScale;

        player.InputHandler.UseBashInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.BashDirectionIndicator.gameObject.SetActive(true);

        player.BashAbleObj.BeforeBash();
        player.BashDirectionIndicator.position = player.BashAbleObj.Transform.position;

        bashDirction = player.FacingDirection * Vector2.one;
        isHolding = true;
        CanBash = false;
        WasBash = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.RB2D.drag = 0.0f;

        if (player.CurrentVelocity.y > playerData.endBashMaxYVelocity)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.endBashYUpMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isHolding)
            {
                bashDirctionInput = player.InputHandler.RawBashDirecionInput;
                bashInputStop = player.InputHandler.BashInputStop;
                player.BashDirectionIndicator.position = player.BashAbleObj.Transform.position;
                xInput = player.InputHandler.NormInputX;
                
                if (bashDirctionInput != Vector2.zero)
                {
                    bashDirction = bashDirctionInput;
                    bashDirction.Normalize();
                }

                player.BashDirectionIndicator.rotation = Quaternion.AngleAxis(Mathf.Atan2(bashDirction.y, bashDirction.x) * Mathf.Rad2Deg, Vector3.forward);
                
                if (bashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1.0f;
                    startTime = Time.time;

                    if (xInput * bashDirction.x >= 0)
                    {
                        player.CheckIfShouldFlip(bashDirction.x > 0 ? 1 : -1);
                    }
                    else
                    {
                        player.CheckIfShouldFlip(xInput > 0 ? 1 : -1);
                    }

                    player.transform.position = player.BashAbleObj.Transform.position;
                    player.SetVelocityZero();
                    player.RB2D.drag = playerData.bashDrag;

                    player.BashDirectionIndicator.gameObject.SetActive(false);

                    player.BashAbleObj.SetImpulse(-bashDirction * playerData.bashImpulse);
                    player.BashAbleObj.AfterBash();
                }
            }
            else
            {
                player.SetVelocity(playerData.bashVelocity, bashDirction);

                if (Time.time >= startTime + playerData.bashTime)
                {
                    isAbilityDone = true;
                }
            }

            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
    }

    public void ResetWasBash()
    {
        WasBash = false;
    }
}
