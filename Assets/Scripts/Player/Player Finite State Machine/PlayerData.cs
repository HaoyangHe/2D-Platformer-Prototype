using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Physics")]
    public float airUpwardGravityScale = 2.0f;
    public float airFallingGravityScale = 2.5f;
    public float bashUpwardGravityScale = 2.0f;
    public float wallJumpUpwardGravityScale = 2.0f;
    public float wallJumpVerticalGravitiScale = 3.0f;

    [Header("Move State")]
    public float movementVelocity = 12.0f;
    public float movementLerp = 1.0f;

    [Header("Jump State")]
    public float jumpVelocityNo1 = 17.0f;
    public float jumpVelocityNo2 = 8.5f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTimeInAir = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float airFlipMultiplier = 0.6f;
    public float airDragDeceleration = 40.0f;
    public float airMovementAcceleration = 40.0f;

    [Header("In Air State Velocity Clamp")]
    public float bashVelocityClamp = 20.0f;

    [Header("Wall Jump Horizontal State")]
    public float wallJumpVelocity = 18.0f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Wall Jump Vertical State")]
    public float wallJumpUpwardVelocity = 18.0f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 8.0f;
    public float wallSlideAcceleration = 5.0f;
    public float getOfWallTime = 0.5f;
    public float coyoteTimeSlide = 0.05f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 5.0f;

    [Header("Ledge Climb State")]
    public Vector2 startOffset = new Vector2(0.35f, 0.95f);
    public Vector2 endOffset = new Vector2(0.4f, 1.0f);

    [Header("Bash State")]
    public float bashVelocity = 30.0f;
    public float bashTime = 0.2f;
    public float bashImpulse = 1.0f;
    public float bashTimeScale = 0.0f;
    public float bashExitXMultiplier = 0.6f;
    public float bashExitYMultiplier = 0.5f;
}
