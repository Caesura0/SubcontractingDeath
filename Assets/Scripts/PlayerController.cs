using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int jumpsAllowed = 2;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float jumpForce = 16f;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] float airDragMultiplier = 0.95f;
    [SerializeField] float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] float wallJumpForce = 0.5f;
    [SerializeField] float jumpTimerSet;
    [SerializeField] float turnTimerSet = 0.1f;
    [SerializeField] float wallJumpTimerSet;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float distanceBetweenImages;
    [SerializeField] float dashCooldown;


    [SerializeField] Vector2 wallHopDirection;
    [SerializeField] Vector2 wallJumpDirection;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;


    [SerializeField] LayerMask whatIsGround;

    [SerializeField] bool masterWallJump;
    [SerializeField] bool masterDash;
    




    bool isFacingRight = true;
    bool isWalking;
    bool isGrounded;
    bool isTouchingWall;
    bool isWallSliding;
    bool canJump;
    bool canWallJump;
    bool isAttemptingJump;
    bool checkJumpMultiplier;
    bool canMove;
    bool canFlip;
    bool hasWallJumped;
    bool isDashing;
    bool isInDialogue = false;


    int jumpsLeft;
    int facingDirection = 1;
    int lastWallJumpDirection;

    float movementInputDirection;
    float jumpTimer;
    float turntimer;
    float wallJumpTimer;
    float dashTimeLeft;
    float lastImageX;
    float lastDash = -100f;


    Rigidbody2D rb;
    Animator animator;
    PlayerConversant playerConversant;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerConversant = GetComponent<PlayerConversant>();
        jumpsLeft = jumpsAllowed;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();

    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void CheckIfWallSliding()
    {//difference !isGrounded

        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }







    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Debug.Log(isGrounded);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0.01f))
        {
            jumpsLeft = jumpsAllowed;
        }


        if (isTouchingWall && masterWallJump)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        canJump = (jumpsLeft > 0);
    }

    void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimation()
    {
        //animator.SetBool("isWalking", isWalking);
        //animator.SetBool("isGrounded", isGrounded);
        //animator.SetFloat("yVelocity", rb.velocity.y);
        //animator.SetBool("isWallSliding", isWallSliding);
    }


    void CheckInput()
    {
        //TODO change to new input system
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && !isInDialogue)
        {
            if (isGrounded || (jumpsLeft > 0 && !isTouchingWall))
            {
                Debug.Log("checkinput normal jump");
                Jump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingJump = true;
            }
        }
        if (Input.GetButtonDown("Horizontal") && isTouchingWall && !isInDialogue)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turntimer = turnTimerSet;
            }
        }
        if (turntimer >= 0)
        {
            turntimer -= Time.deltaTime;
            if (turntimer <= 0)
            {

                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash") && !isInDialogue)
        {
            if (Time.time >= (lastDash + dashCooldown) && masterDash)
            {
                AttemptToDash();
            }
        }
        if (Input.GetButtonDown("Fire2") && playerConversant.GetConversant() != null)
        {
            playerConversant.StartDialogue();
        }
    }


    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageX = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }
    void CheckDash()
    {
        if (isDashing)
        {

            if (dashTimeLeft > 0)
            {

                canMove = false;
                canFlip = false;


                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageX) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageX = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }

    }
    void CheckJump()
    {
        if (jumpTimer > 0)
        {
            //walljump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection && masterWallJump)
            {
                Debug.Log("wall jump???!!!!");
                WallJump();
            }
            else if (isGrounded)
            {
                Debug.Log("check jump normal jump");
                Jump();
            }
        }
        if (isAttemptingJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsLeft--;
            jumpTimer = 0;
            isAttemptingJump = false;
            checkJumpMultiplier = true;
        }
    }

    void WallJump()
    {
        if (canWallJump && masterWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            jumpsLeft = jumpsAllowed;
            jumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingJump = false;
            checkJumpMultiplier = true;
            turntimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }




    void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }


        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }



    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }




    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

    }
}
