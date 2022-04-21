using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int attackPower = 20;
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float rayCastLength = 2.0f;
    [SerializeField] private Vector2 rayCastOffset;
    [SerializeField] private LayerMask rayCastLayerMask;

    private RaycastHit2D ledgeRaycastHit;
    private RaycastHit2D wallRaycastHit;
    private Vector2 ledgeRayStartPos;
    private int facingDirection = 1;
    private int health;

    private void Start() 
    {
        health = maxHealth;
    }

    void Update()
    {
        ledgeRayStartPos = new Vector2(transform.position.x + facingDirection * rayCastOffset.x, transform.position.y + rayCastOffset.y);
        ledgeRaycastHit = Physics2D.Raycast(ledgeRayStartPos, Vector2.down, rayCastLength, rayCastLayerMask);
        wallRaycastHit = Physics2D.Raycast((Vector2)transform.position, facingDirection * Vector2.right, rayCastLength, rayCastLayerMask);
        
        if (!ledgeRaycastHit || wallRaycastHit)
        {
            Flip();
        }

        targetVelocity = new Vector2(maxSpeed * facingDirection, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject == NewPlayer.Instance.gameObject)
        {
            NewPlayer.Instance.Damage(attackPower);
        }
    }

    private void OnDrawGizmos() 
    {
        ledgeRayStartPos = new Vector2(transform.position.x + facingDirection * rayCastOffset.x, transform.position.y + rayCastOffset.y);
        Gizmos.DrawLine(ledgeRayStartPos, ledgeRayStartPos + Vector2.down * rayCastLength);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + facingDirection * Vector2.right * rayCastLength);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
