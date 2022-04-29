using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 12.0f;
    public float movementLerp = 1.0f;

    [Header("Jump State")]
    public float jumpVelocity = 17.0f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTimeInAir = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float abilityInterruptMultiplier = 0.8f;
    public float fallingMultiplier = 2.5f;
    public float airFlipMultiplier = 0.3f;
    public float airDragDeceleration = 60.0f;
    public float airMovementAcceleration = 45.0f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20.0f;
    public float wallJumpVerticalVelocity = 10.0f;
    public float wallJumpFallingMultiplier = 4.0f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Wall Slide State")]
    public float coyoteTimeSlide = 0.1f;
    public float wallSlideVelocity = 2.0f;
    public float getOfWallTime = 0.5f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3.0f;

    [Header("Ledge Climb State")]
    public Vector2 startOffset = new Vector2(0.35f, 0.95f);
    public Vector2 endOffset = new Vector2(0.4f, 1.0f);

    [Header("Bash State")]
    public float bashVelocity = 30.0f;
    public float bashTime = 0.2f;
    public float bashImpulse = 10.0f;
    public float bashTimeScale = 0.0f;
    public float bashExitXMultiplier = 0.8f;
    public float bashExitYMultiplier = 0.7f;
    public float bashFallingMultiplier = 8.0f;
}
