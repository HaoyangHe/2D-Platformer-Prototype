public class PlayerGroundedState : PlayerState
{
    // Player Inputs
    protected int xInput;
    protected bool jumpInput;
    private bool grabInput;

    // Collision Senses
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    public PlayerGroundedState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;

        if (jumpInput && player.JumpState.CanJump())    // Player presses the JumpButton.
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)   // Player walks to the edge and falls.
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingLedge && isTouchingWall && grabInput)    // Player walks to a wall and presses GrabButton.
        {                                                           // NOTE: Adding isTouchingLedge makes sure that the wall is tall enougth to grab.
            stateMachine.ChangeState(player.WallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenser.IsGrounded;
        isTouchingWall = collisionSenser.IsTouchingWallFront;
        isTouchingLedge = collisionSenser.IsTouchingLedge;
    }
}
