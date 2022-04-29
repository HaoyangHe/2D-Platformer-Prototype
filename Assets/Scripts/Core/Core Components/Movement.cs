using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB2D { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();
        RB2D = GetComponentInParent<Rigidbody2D>();         // Find the Rigidbody2D that attached to the player. 
        FacingDirection = 1;
    }

    public void LogicUpdate()
    { 
        CurrentVelocity = RB2D.velocity;
    }

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        direction.Normalize();
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void AddVelocityX(float velocity)
    {
        workspace.Set(CurrentVelocity.x + velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void DecreaseVelocityX(float velocity)
    {
        if (CurrentVelocity.x > 0)
        {
            workspace.Set(Mathf.Clamp(CurrentVelocity.x - velocity, 0.0f, CurrentVelocity.x), CurrentVelocity.y);
        }
        else
        {
            workspace.Set(Mathf.Clamp(CurrentVelocity.x - velocity, CurrentVelocity.x, 0.0f), CurrentVelocity.y);
        }
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void AddVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, CurrentVelocity.y + velocity);
        SetFinalVelocity();
    }

    public void DecreaseVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, CurrentVelocity.y - velocity);
        SetFinalVelocity();
    }

    public void AddForce(Vector2 forceToAdd)
    {
        RB2D.AddForce(forceToAdd);
    }

    public void AddForceImpulse(Vector2 forceToAdd)
    {
        RB2D.AddForce(forceToAdd, ForceMode2D.Impulse);
    }

    public bool CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
            return true;
        }

        return false;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB2D.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SetFinalVelocity()
    {
        RB2D.velocity = workspace;
        CurrentVelocity = workspace;
    }
}
