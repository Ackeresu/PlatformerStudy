using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private InputActions input;
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D jumpingCollider;

    public float topSpeed = 2f;
    public float topRunSpeed = 3f;
    public float acceleration = 1f;
    public float jumpHeight = 4f;
    public float airJumpHeight = 3f;
    public int maxAirJumps = 1;
    public float dashLenght = 2f;
    
    private Rigidbody2D body;
    private Animator anim;
    private Vector2 playerMovement;
    private float moveSpeed;
    private int availableAirJumps;
    private float movementInputX;
    //private float movementInputY;

    private bool isRunning;

    // Ground check
    private CheckGroundedState groundedState;
    private bool shouldCheckGround;

    public Platform platform;

    // Animation
    private static string SPEED = "Speed";
    private static string IS_JUMPING = "isJumping";
    //private static string IS_CROUCHING = "isCrouching";

    //private void OnDrawGizmos() {
    //    Vector3 max = standingCollider.bounds.max;
    //    Vector3 min = standingCollider.bounds.min;
    //    Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
    //    Vector2 corner2 = new Vector2(min.x, min.y - 0.15f);
    //
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(max, min);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(corner1, corner2);
    //}

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundedState = GetComponentInChildren<CheckGroundedState>();

        moveSpeed = 0;
        availableAirJumps = maxAirJumps;

        input.OnPlayerMovement += HandleMovement;
        input.OnPlayerRun += HandleRunning;
        input.OnPlayerJump += HandleJumping;
        input.OnPlayerDash += HandleDashing;
    }

    private void Update() {
        UpdateMovement();
        HandleMovingPlatforms();
        HandlePlayerScale();
        GroundCheck();
    }

    private void UpdateMovement() {
        if (!isRunning) {
            if (movementInputX == 0) {
                moveSpeed = 0;
            }
            if (moveSpeed > topSpeed) {
                moveSpeed = topSpeed;
            }
            if (moveSpeed <= topSpeed) {
                moveSpeed += acceleration * Time.deltaTime;
                playerMovement = new Vector2(movementInputX * moveSpeed, body.velocity.y);
            } else {
                playerMovement = new Vector2(movementInputX * topSpeed, body.velocity.y);
            }
        } else {
            if (movementInputX == 0) {
                moveSpeed = 0;
            }
            if (moveSpeed <= topRunSpeed) {
                moveSpeed += acceleration * Time.deltaTime;
                playerMovement = new Vector2(movementInputX * moveSpeed, body.velocity.y);
            } else {
                playerMovement = new Vector2(movementInputX * topRunSpeed, body.velocity.y);
            }
        }
        body.velocity = playerMovement;
    }

    private void HandleMovingPlatforms() {
        // Create an area under the player to check if it collides with platform
        Vector3 max = standingCollider.bounds.max;
        Vector3 min = standingCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.15f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        platform = null;

        if (hit != null) {
            platform = hit.GetComponent<Platform>();
        } else {
            transform.parent = null;
        }
        if (platform != null) {
            transform.parent = platform.transform;
        } else {
            transform.parent = null;
        }
    }

    private void HandlePlayerScale() {
        Vector3 playerScale = Vector3.one;

        if (platform != null) {
            playerScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x) / playerScale.x, 1 / playerScale.y, 1);
        }
        anim.SetFloat(SPEED, Mathf.Abs(playerMovement.x));
    }

    private void GroundCheck() {
        if (body.velocity.y <= 0) {
            shouldCheckGround = true;
        }
        if (shouldCheckGround && groundedState.GetIsGrounded()) {
            standingCollider.enabled = true;
            jumpingCollider.enabled = false;

            anim.SetBool(IS_JUMPING, false);

            shouldCheckGround = false;

            availableAirJumps = maxAirJumps;
        }
    }

//========================================================================================

    private void HandleMovement(object sender, Vector2 movementInput) {
        movementInputX = movementInput.x;
    }

    private void HandleRunning(object sender, bool runningInput) {
        if (runningInput) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }

    private void HandleJumping(object sender, EventArgs empty) {
        if (groundedState.GetIsGrounded()) {
            // Reset the Y momentum before applying the force
            Vector2 newVelocity = new Vector2(body.velocity.x, 0);
            body.velocity = newVelocity;
            body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

            standingCollider.enabled = false;
            jumpingCollider.enabled = true;

            anim.SetBool(IS_JUMPING, true);
        }
        if (!groundedState.GetIsGrounded() && availableAirJumps > 0) {
            // Reset the Y momentum before applying the force
            Vector2 newVelocity = new Vector2(body.velocity.x, 0);
            body.velocity = newVelocity;
            body.AddForce(Vector2.up * airJumpHeight, ForceMode2D.Impulse);

            availableAirJumps -= 1;

            anim.SetBool(IS_JUMPING, true);
        }
    }

    private void HandleDashing(object sender, EventArgs empty) {
        //if (!groundedState.GetIsGrounded()) {
            Debug.Log("dashing");
            //body.AddRelativeForce(Vector2.forward * dashLenght, ForceMode2D.Impulse);
        //}
    }

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovement;
        input.OnPlayerRun -= HandleRunning;
        input.OnPlayerJump -= HandleJumping;
        input.OnPlayerDash -= HandleDashing;
    }
}
