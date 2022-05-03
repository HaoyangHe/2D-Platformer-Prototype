using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; private set => wallCheck = value; }
    public Transform LedgeCheck { get => ledgeCheck; private set => ledgeCheck = value; }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [Header("Collision Check Transforms")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;

    [Header("Collision Check Properties")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float bashCheckRadius;
    
    [Header("Collision LayerMask")]
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

    public bool IsNearBashAble
    {
        get 
        {
            Collider2D obj = Physics2D.OverlapCircle(transform.position, bashCheckRadius, whatIsInteractable);
            
            if (obj == null)
            {
                BashAbleObj?.Unlit();
                BashAbleObj = null;
                return false;
            }
            else
            {
                BashAbleObj = obj.GetComponent<BashableObject>();
                BashAbleObj?.Lit();
                return true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        // Gizmos.DrawWireSphere(transform.position, bashCheckRadius);
        if (core != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + core.Movement.FacingDirection * wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
            Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + core.Movement.FacingDirection * wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        }
    }
}
