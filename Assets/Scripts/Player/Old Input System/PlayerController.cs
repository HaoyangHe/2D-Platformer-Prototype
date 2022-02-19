using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float jumpOfWallTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100.0f;
    private float startDashPos;
    private float startDashXVelocity;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private const float minRecognisedMoving = 0.001f;

    // Player states triggers
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool hasWallJumpedSameDir;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool canMove = true;
    private bool canFlip = true;
    private bool isDashing;
    private bool canDash = true;
    private bool attemptToDash;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    // Component references
    private Rigidbody2D rb;
    private Animator  anim;

    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float jumpOfWallTimerSet = 0.65f;
    public float wallJumpTimerSet = 0.35f;
    public float ledgeClimbXOffset1 = 0.0f;
    public float ledgeClimbYOffset1 = 0.0f;
    public float ledgeClimbXOffset2 = 0.0f;
    public float ledgeClimbYOffset2 = 0.0f;
    public float dashTimeSet = 0.2f;
    public float dashSpeed = 50.0f;
    public float dashDistance = 10.0f;
    public float distanceBetweenImages = 0.1f;
    public float dashCoolDown = 1.0f;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckIfCanDash();
        CheckSurroundings();
        CheckDash();
        UpdateAnimations();
    }

    private void FixedUpdate() 
    {
       ApplyMovement();
       CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < -minRecognisedMoving && !canClimbLedge)
        {
            isWallSliding = true;
            
            if (jumpOfWallTimer < 0)
            {
                isWallSliding = false;
                jumpOfWallTimer = jumpOfWallTimerSet;
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else 
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;
            anim.SetBool("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    public void FinishLedegClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && Mathf.Abs(rb.velocity.y) <= minRecognisedMoving)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

        if (hasWallJumpedSameDir && wallJumpTimer > 0)
        {
            canNormalJump = false;
            wallJumpTimer -= Time.deltaTime;
        }
        else
        {
            hasWallJumpedSameDir = false;
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > minRecognisedMoving)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (isWallSliding && movementInputDirection == -facingDirection)
        {
            jumpOfWallTimer -= Time.deltaTime;
        }
        else if (isTouchingWall)
        {
            jumpOfWallTimer = jumpOfWallTimerSet;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (canNormalJump && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump") && rb.velocity.y > minRecognisedMoving)
        {
            rb.velocity = new Vector2(rb.velocity.x, variableJumpHeightMultiplier * rb.velocity.y);
            checkJumpMultiplier = false;
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (!isDashing && Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();
                attemptToDash = true;
            }
        }

        InputTrigger();
    }

    private void InputTrigger()
    {
        if (canDash && attemptToDash && isTouchingWall && movementInputDirection * facingDirection < 0)
        {
            attemptToDash = false;
            isTouchingWall = false;
        }
    }

    private void AttemptToDash()
    {
        if (canDash)
        {
            isDashing = true;
            isWallSliding = false;
            dashTimeLeft = dashTimeSet;
            lastDash = Time.time;
            startDashPos = transform.position.x;
            startDashXVelocity = rb.velocity.x;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
    }

    private void CheckIfCanDash()
    {
        if (canClimbLedge || (isTouchingWall && movementInputDirection != -facingDirection))
        {
            canDash = false;

            if (!canClimbLedge)
            {
                isDashing = false;
                attemptToDash = false;
                canMove = true;
                canFlip = true;
            }
        }
        else
        {
            canDash = true;
        }
    }

    private void CheckDash()
    {
        if (canDash && isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(facingDirection * dashSpeed, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) >= distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (Mathf.Abs(transform.position.x - startDashPos) >= dashDistance)
            {
                rb.velocity = new Vector2(startDashXVelocity, rb.velocity.y);
                dashTimeLeft = -1;
            }

            if (dashTimeLeft <= 0 || (isTouchingWall && movementInputDirection != -facingDirection))
            {
                attemptToDash = false;
                isDashing = false;
                canMove = true;
                canFlip = true;
            } 
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && movementInputDirection != 0)
            {
                WallJump();
            }
            else if (!isGrounded && isTouchingWall && movementInputDirection == 0)
            {
                WallHop();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump && jumpTimer >= 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallHop()
    {
        if (canWallJump)
        {
            Vector2 forceToAdd = new Vector2(-facingDirection * wallHopDirection.x, wallHopDirection.y) * wallHopForce;
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            amountOfJumpsLeft = amountOfJumps - 1;
            
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            Vector2 forceToAdd;
            if (movementInputDirection == facingDirection)
            {
                forceToAdd = new Vector2(-movementInputDirection * wallJumpDirection.x, wallJumpDirection.y) * wallJumpForce;
                wallJumpTimer = wallJumpTimerSet;
                hasWallJumpedSameDir = true;
            }
            else
            {
                forceToAdd = new Vector2(movementInputDirection * wallJumpDirection.x, wallJumpDirection.y) * wallJumpForce;
            }
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            amountOfJumpsLeft = amountOfJumps - 1;

            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            if (isGrounded && Mathf.Abs(rb.velocity.y) <= minRecognisedMoving)
            {
                rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
            }
            else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
            {
                Vector2 forceToAdd = new Vector2(movementInputDirection * movementForceInAir, 0.0f);
                rb.AddForce(forceToAdd);

                if (isFacingRight && rb.velocity.x > movementSpeed)
                {
                    rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
                }
                else if (!isFacingRight && rb.velocity.x < -movementSpeed)
                {
                    rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                }
            }
            else if (!isGrounded && !isWallSliding && movementInputDirection == 0)
            {
                rb.velocity = new Vector2(airDragMultiplier * rb.velocity.x, rb.velocity.y);
            }

            if (isWallSliding)
            {
                if (rb.velocity.y < -wallSlideSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                }
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180f, 0.0f);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDirection * wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + facingDirection * wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        Gizmos.DrawLine(ledgePos1, ledgePos2);
        Gizmos.DrawWireCube(ledgePos1, new Vector3(0.3f, 0.3f, 0.1f));
        Gizmos.DrawWireCube(ledgePos2, new Vector3(0.3f, 0.3f, 0.1f));
    }
}
