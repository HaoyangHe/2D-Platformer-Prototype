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

    public PlayerBashState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
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

        core.CollisionSenses.BashAbleObj.BeforeBash();
        player.BashDirectionIndicator.position = core.CollisionSenses.BashAbleObj.Transform.position;

        bashDirction = core.Movement.FacingDirection * Vector2.one;
        isHolding = true;
        CanBash = false;
        WasBash = true;
    }

    public override void Exit()
    {
        base.Exit();

        movementAPI.RB2D.drag = 0.0f;

        if (core.Movement.CurrentVelocity.y > playerData.endBashMaxYVelocity)
        {
            
            core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.endBashYUpMultiplier);
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
                player.BashDirectionIndicator.position = core.CollisionSenses.BashAbleObj.Transform.position;
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
                        core.Movement.CheckIfShouldFlip(bashDirction.x > 0 ? 1 : -1);
                    }
                    else
                    {
                        core.Movement.CheckIfShouldFlip(xInput > 0 ? 1 : -1);
                    }

                    player.transform.position = core.CollisionSenses.BashAbleObj.Transform.position;
                    core.Movement.SetVelocityZero();
                    movementAPI.RB2D.drag = playerData.bashDrag;

                    player.BashDirectionIndicator.gameObject.SetActive(false);

                    core.CollisionSenses.BashAbleObj.SetImpulse(-bashDirction * playerData.bashImpulse);
                    core.CollisionSenses.BashAbleObj.AfterBash();
                }
            }
            else
            {
                core.Movement.SetVelocity(playerData.bashVelocity, bashDirction);

                if (Time.time >= startTime + playerData.bashTime)
                {
                    isAbilityDone = true;
                }
            }

            player.Anim.SetFloat("xVelocity", Mathf.Abs(core.Movement.CurrentVelocity.x));
            player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.IsGrounded;
        isTouchingWall = core.CollisionSenses.IsTouchingWallFront;
    }

    public void ResetWasBash()
    {
        WasBash = false;
    }
}
