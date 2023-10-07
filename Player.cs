using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerAnimations))]

public class Player : MonoBehaviour {

    [SerializeField] private InputActions input;
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D ballCollider;

    public int health = 5;
    public float topSpeed = 2f;
    public float topRunSpeed = 3f;
    public float acceleration = 1f;
    public float jumpHeight = 4f;
    public float airJumpHeight = 3f;
    public int maxAirJumps = 1;

    private float movementInputX;
    private float movementInputY;
    private int availableAirJumps;
    private float minLockTime = 0.2f;
    private Rigidbody2D body;
    private Vector2 playerMovement;

    public bool isJumping;

    private bool canMove = true;
    private bool isRunning;
    private bool isCrouching;
    private bool isRaising;
    private bool isFalling;
    private bool isHit;

    public BoxCollider2D playerCollider;
    private Effects effects;
    private GroundChecker groundChecker;
    private PlayerAnimations playerAnimations;
    public Platform platform;

    public bool GetIsJumping() => isJumping;
    public bool GetIsHit() => isHit;

    //========================================================================================

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        playerAnimations = GetComponent<PlayerAnimations>();

        input.OnPlayerMovement += HandleMovementInput;
        input.OnPlayerRun += HandleRunning;
        input.OnPlayerJump += HandleJumping;

        availableAirJumps = maxAirJumps;
    }

    private void Update() {
        GroundCheck();
        UpdateMovement();
        UpdateCollider();
        HandlePlayerScale();
        HandlePlayerAnimations();
    }

    //========================================================================================

    private void UpdateMovement() {
        if (!canMove) {
            return;
        }
        if (!isRunning) {
            playerMovement = new Vector2(movementInputX * topSpeed, 0);
            Move(topSpeed);
        } else {
            playerMovement = new Vector2(movementInputX * topRunSpeed, 0);
            Move(topRunSpeed);
        }
        if (movementInputY < 0) {
            isCrouching = true;
        } else {
            isCrouching = false;
        }
        GetPlayerVelocityY();
    }

    private void UpdateCollider() {
        if (groundChecker.GetIsGrounded()) {
            standingCollider.enabled = true;
            ballCollider.enabled = false;

            //playerCollider = standingCollider;
        }
        if (isHit) {
            //standingCollider.enabled = false;
            //ballCollider.enabled = false;
        }
        if (isJumping || isCrouching) {
            standingCollider.enabled = false;
            ballCollider.enabled = true;

            //playerCollider = ballCollider;
        }
    }

    private void HandlePlayerScale() {
        Vector3 playerScale = Vector3.one;

        if (groundChecker.lastPlatform != null) {
            playerScale = groundChecker.lastPlatform.transform.localScale;
        }
        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x) / playerScale.x, 1 / playerScale.y, 1);
        }
        //playerAnimations.MoveAnimation(playerMovement.x);
    }

    private void GroundCheck() {
        if (body.velocity.y <= 0.1 && groundChecker.GetIsGrounded()) {
            availableAirJumps = maxAirJumps;
            isJumping = false;
        }
    }

    private void GetPlayerVelocityY() {
        if (body.velocity.y >= 0.1 && !groundChecker.GetIsGrounded()) {
            isRaising = true;
        } else {
            isRaising = false;
        }
        if (body.velocity.y <= 0.1 && !groundChecker.GetIsGrounded()) {
            isFalling = true;
        } else {
            isFalling = false;
        }
    }

    //========================================================================================

    public void LockMovement(float lockTime) {
        if (!canMove) {
            return;
        }
        canMove = false;
        StartCoroutine(Acker.Utility.ActionAfterTimer.ActionAfterWaiting(delegate { canMove = true; }, lockTime));
    }

    public IEnumerator LockMovementUntilGrounded() {
        isHit = true;

        new WaitForSeconds(minLockTime);

        while (!groundChecker.GetIsGrounded()) {
            canMove = false;
            yield return null;
        }
        canMove = true;
        isHit = false;
    }

    public void ResetMomentum() {
        Vector2 newVelocity = new Vector2(0, 0);
        body.velocity = newVelocity;
    }

    //========================================================================================

    private void HandleMovementInput(object sender, Vector2 movementInput) {
        movementInputX = movementInput.x;
        movementInputY = movementInput.y;
    }

    private void Move(float maxSpeed) {
        body.AddForce(playerMovement, ForceMode2D.Force);

        if (body.velocity.x > maxSpeed) {
            body.velocity = new Vector2(maxSpeed, body.velocity.y);
        } else if (body.velocity.x < -maxSpeed) {
            body.velocity = new Vector2(-maxSpeed, body.velocity.y);
        }
    }

    private void HandleRunning(object sender, bool runningInput) {
        if (runningInput) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }

    private void HandleJumping(object sender, EventArgs empty) {
        if (!canMove) {
            return;
        }
        if (groundChecker.GetIsGrounded()) {
            Jump(body.velocity.x, 0, Vector2.up * jumpHeight);
            isJumping = true;
        }
        if (!groundChecker.GetIsGrounded() && availableAirJumps > 0) {
            Jump(body.velocity.x, 0, Vector2.up * airJumpHeight);
            availableAirJumps--;
            isJumping = true;
        }
    }

    public void Jump(float velX, float velY, Vector2 jumpForce) {
        Vector2 newVelocity = new Vector2(velX, velY);
        body.velocity = newVelocity;

        body.AddForce(jumpForce, ForceMode2D.Impulse);
    }

    private void HandlePlayerAnimations() {
        // Movement
        playerAnimations.MoveAnimation(playerMovement.x);

        // Crouch
        if (isCrouching) {
            playerAnimations.CrouchingAnimation(1, isCrouching);
        } else {
            playerAnimations.CrouchingAnimation(-1, isCrouching);
        }

        // Jump
        if (isJumping) {
            playerAnimations.JumpAnimation(isJumping);
        } else {
            playerAnimations.JumpAnimation(isJumping);
        }

        // Hit
        if (isHit) {
            playerAnimations.HitAnimation(isHit);
        } else {
            playerAnimations.HitAnimation(isHit);
        }

        // Raising and Falling
        if (isRaising && !isJumping) {
            playerAnimations.RaisingAnimation(isRaising);
        } else {
            playerAnimations.RaisingAnimation(isRaising);
        }
        if (isFalling && !isJumping) {
            playerAnimations.FallingAnimation(isFalling);
        } else {
            playerAnimations.FallingAnimation(isFalling);
        }
    }

    //========================================================================================

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovementInput;
        input.OnPlayerRun -= HandleRunning;
        input.OnPlayerJump -= HandleJumping;
    }
}
