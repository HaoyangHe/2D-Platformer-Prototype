using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; private set => wallCheck = value; }
    public Transform LedgeCheck { get => ledgeCheck; private set => ledgeCheck = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float bashCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsInteractable;

    public BashableObject BashAbleObj { get; private set; }

    public bool IsGrounded
    {
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool IsTouchingWallFront
    {
        get => Physics2D.Raycast(wallCheck.position, core.Movement.FacingDirection * Vector2.right, wallCheckDistance, whatIsGround);
    }

    public bool IsTouchingWallBack
    {
        get => Physics2D.Raycast(wallCheck.position, -core.Movement.FacingDirection * Vector2.right, wallCheckDistance, whatIsGround);
    }

    public bool IsTouchingLedge
    {
        get => Physics2D.Raycast(ledgeCheck.position, core.Movement.FacingDirection * Vector2.right, wallCheckDistance, whatIsGround);
    }

    public bool CheckIfNearBashAble()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bashCheckRadius, whatIsInteractable);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "BashAble")
            {
                BashAbleObj = hit.transform.gameObject.GetComponent<BashableObject>();
                BashAbleObj.Lit();
                return true;
            }
        }

        if (BashAbleObj != null)
        {
            BashAbleObj.Unlit();
            BashAbleObj = null;
        }

        return false;
    }
}
