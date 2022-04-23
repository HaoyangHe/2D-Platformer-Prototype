using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : PhysicsObject
{
    [SerializeField] private int maxHP = 50;
    [SerializeField] private int attackPower = 20;
    [SerializeField] private float speedX = 1.0f;
    [SerializeField] private float rayCastLength = 2.0f;
    [SerializeField] private Vector2 ledgeRayCastOffset;
    [SerializeField] private LayerMask whatIsGround;

    private RaycastHit2D ledgeRaycastHit;
    private RaycastHit2D wallRaycastHit;
    private Vector2 ledgeRayStartPos;
    private int facingDirection = 1;
    private int health;

    private void Start() 
    {
        health = maxHP;
    }

    private void Patrolling()
    {
        ledgeRayStartPos.Set(transform.position.x + facingDirection * ledgeRayCastOffset.x, transform.position.y + ledgeRayCastOffset.y);
        ledgeRaycastHit = Physics2D.Raycast(ledgeRayStartPos, Vector2.down, rayCastLength, whatIsGround);
        wallRaycastHit = Physics2D.Raycast((Vector2)transform.position, facingDirection * Vector2.right, rayCastLength, whatIsGround);
        
        if (!ledgeRaycastHit || wallRaycastHit)
        {
            Flip();
        }
    }

    protected override void ComputeVelocity()
    {
        Patrolling();

        targetVelocity.Set(speedX * facingDirection, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.callbacks.Damage(attackPower);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos() 
    {
        ledgeRayStartPos.Set(transform.position.x + facingDirection * ledgeRayCastOffset.x, transform.position.y + ledgeRayCastOffset.y);
        Gizmos.DrawLine(ledgeRayStartPos, ledgeRayStartPos + Vector2.down * rayCastLength);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + facingDirection * Vector2.right * rayCastLength);
    }
}
