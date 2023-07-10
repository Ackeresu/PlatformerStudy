using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private InputActions input;
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D jumpingCollider;

    public int health = 5;
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

    //private bool isJumping;
    private bool isRunning;

    // Ground check
    private GroundChecker groundChecker;
    private bool shouldCheckGround;

    public Platform platform;
    public Enemy enemy;

    // Animation
    private static string SPEED = "Speed";
    private static string IS_JUMPING = "isJumping";
    //private static string IS_CROUCHING = "isCrouching";

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundChecker = GetComponentInChildren<GroundChecker>();

        moveSpeed = 0;
        availableAirJumps = maxAirJumps;

        input.OnPlayerMovement += HandleMovement;
        input.OnPlayerRun += HandleRunning;
        input.OnPlayerJump += HandleJumping;
        input.OnPlayerDash += HandleDashing;
    }

    private void Update() {
        UpdateMovement();
        HandlePlayerScale();
        HandleEnemiesCollision();
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

    private void HandlePlayerScale() {
        Vector3 playerScale = Vector3.one;

        if (groundChecker.lastPlatform != null) {
            playerScale = groundChecker.lastPlatform.transform.localScale;
        }
        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x) / playerScale.x, 1 / playerScale.y, 1);
        }
        anim.SetFloat(SPEED, Mathf.Abs(playerMovement.x));
    }


    private void HandleEnemiesCollision() {

    }

    private void GroundCheck() {
        if (body.velocity.y <= 0 && groundChecker.GetIsGrounded()) {
            standingCollider.enabled = true;
            jumpingCollider.enabled = false;
        
            availableAirJumps = maxAirJumps;

            HandleJumpAnimation(false);
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
        if (groundChecker.GetIsGrounded()) {
            // Reset the Y momentum before applying the force
            Vector2 newVelocity = new Vector2(body.velocity.x, 0);
            body.velocity = newVelocity;
            body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

            standingCollider.enabled = false;
            jumpingCollider.enabled = true;

            HandleJumpAnimation(true);
        }
        if (!groundChecker.GetIsGrounded() && availableAirJumps > 0) {
            // Reset the Y momentum before applying the force
            Vector2 newVelocity = new Vector2(body.velocity.x, 0);
            body.velocity = newVelocity;
            body.AddForce(Vector2.up * airJumpHeight, ForceMode2D.Impulse);

            availableAirJumps--;

            HandleJumpAnimation(true);
        }
    }

    private void HandleDashing(object sender, EventArgs empty) {
        //if (!groundedState.GetIsGrounded()) {
        //Debug.Log("dashing");
        //body.AddRelativeForce(Vector2.forward * dashLenght, ForceMode2D.Impulse);
        //}
    }

    //========================================================================================
    private void HandleJumpAnimation(bool isJumping) {
        anim.SetBool(IS_JUMPING, isJumping);
    }

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovement;
        input.OnPlayerRun -= HandleRunning;
        input.OnPlayerJump -= HandleJumping;
        input.OnPlayerDash -= HandleDashing;
    }
}
