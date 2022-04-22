using UnityEngine;

public class PresetPlayerMovement : PhysicsObject
{
    [SerializeField] 
    private PresetPlayerData playerData;
    
    private int facingDirection = 1;        // Facing right by default
    private bool jumpInput;
    private float inputX;
    private Vector2 workSpace;

    protected override void ComputeVelocity()
    {
        inputX = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        
        CheckIfShouldFlip();
        
        // Apply movement
        workSpace.Set(inputX * playerData.maxHorizontalSpeed, 0);
        targetVelocity = workSpace;
        
        if (grounded && jumpInput)
        {
            velocity.y = playerData.jumpVelocity;
        }
    }

    private bool CheckIfShouldFlip()
    {
        if (Mathf.Abs(inputX) > playerData.MINRECONISEDMOVEMENT && inputX * facingDirection < 0)
        {
            Flip();
            return true;
        }

        return false;
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
