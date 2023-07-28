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
    //public float dashLenght = 2f;

    private float movementInputX;
    private float movementInputY;
    private int availableAirJumps;
    private float minLockTime = 0.1f;
    private Rigidbody2D body;
    private Vector2 playerMovement;

    private bool canMove = true;
    private bool isRunning;
    private bool isJumping;
    private bool gotHit = false;

    private GroundChecker groundChecker;

    private PlayerAnimations playerAnimations;

    public Platform platform;
    public Enemy enemy;

    private const string ENEMY = "Enemy";

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        playerAnimations = GetComponent<PlayerAnimations>();

        availableAirJumps = maxAirJumps;

        input.OnPlayerMovement += HandleMovement;
        input.OnPlayerRun += HandleRunning;
        input.OnPlayerJump += HandleJumping;
        input.OnPlayerDash += HandleDashing;
    }

    private void Update() {
        UpdateMovement();
        HandlePlayerScale();
        GroundCheck();
    }

    //========================================================================================

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (LayerMask.LayerToName(collision.gameObject.layer) == ENEMY) {
    //        enemy = collision.gameObject.GetComponent<Enemy>();
    //
    //        if (isJumping) {
    //            EnemyHit();
    //        } else {
    //            EnemyCollision();
    //        }
    //    }
    //}
    private void OnCollisionStay2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == ENEMY) {
            enemy = collision.gameObject.GetComponent<Enemy>();

            if (isJumping) {
                EnemyHit();
            } else {
                EnemyCollision();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == ENEMY) {
            enemy = null;
        }
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
    }

    private void HandlePlayerScale() {
        Vector3 playerScale = Vector3.one;

        if (groundChecker.lastPlatform != null) {
            playerScale = groundChecker.lastPlatform.transform.localScale;
        }
        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x) / playerScale.x, 1 / playerScale.y, 1);
        }
        playerAnimations.MoveAnimation(playerMovement.x);
    }

    private void GroundCheck() {
        // Is grounded
        if (body.velocity.y <= 0 && groundChecker.GetIsGrounded()) {
            standingCollider.enabled = true;
            jumpingCollider.enabled = false;

            availableAirJumps = maxAirJumps;

            isJumping = false;
            playerAnimations.JumpAnimation(false);
        }
        // Is raising
        if (body.velocity.y >= 0.1f && !groundChecker.GetIsGrounded()) {
            playerAnimations.RaisingAnimation(true);
        } else {
            playerAnimations.RaisingAnimation(false);
        }
        // Is falling
        if (body.velocity.y <= 0.1f && !groundChecker.GetIsGrounded()) {
            playerAnimations.FallingAnimation(true);
        } else {
            playerAnimations.FallingAnimation(false);
        }
    }

    private void EnemyHit() {
        Vector2 knockback = new Vector2(body.velocity.x, Mathf.Abs(body.velocity.y));

        body.AddForce(knockback, ForceMode2D.Impulse);

        enemy.gameObject.SetActive(false);
    }

    private void EnemyCollision() {
        ResetMomentum();

        Vector2 knockback = new Vector2(transform.localScale.x * enemy.horizontalKnockback, enemy.verticalKnockback);

        body.AddForce(knockback, ForceMode2D.Impulse);

        if (!gotHit) {
            StartCoroutine(LockMovementUntilGrounded());
        }
    }

    public void LockMovement(float lockTime) {
        if (!canMove) {
            return;
        }
        canMove = false;
        StartCoroutine(Acker.Utility.ActionAfterTimer.ActionAfterWaiting(delegate { canMove = true; }, lockTime));
    }

    private IEnumerator LockMovementUntilGrounded() {
        gotHit = true;

        playerAnimations.HitAnimation(gotHit);

        new WaitForSeconds(minLockTime);

        while (!groundChecker.GetIsGrounded()) {
            canMove = false;
            yield return null;
        }
        canMove = true;
        gotHit = false;

        playerAnimations.HitAnimation(gotHit);
    }

    //========================================================================================

    private void HandleMovement(object sender, Vector2 movementInput) {
        movementInputX = movementInput.x;
        movementInputY = movementInput.y;

        if (movementInputY < 0) {
            playerAnimations.CrouchingAnimation(1, true);
        } else {
            playerAnimations.CrouchingAnimation(-1, false);
        }
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

            standingCollider.enabled = false;
            jumpingCollider.enabled = true;

            isJumping = true;
            playerAnimations.JumpAnimation(true);
        }
        if (!groundChecker.GetIsGrounded() && availableAirJumps > 0) {
            Jump(body.velocity.x, 0, Vector2.up * airJumpHeight);

            availableAirJumps--;

            isJumping = true;
            playerAnimations.JumpAnimation(true);
        }
    }

    public void Jump(float velX, float velY, Vector2 jumpForce) {
        Vector2 newVelocity = new Vector2(velX, velY);
        body.velocity = newVelocity;

        body.AddForce(jumpForce, ForceMode2D.Impulse);
    }

    private void HandleDashing(object sender, EventArgs empty) {
        //if (!groundedState.GetIsGrounded()) {
        //Debug.Log("dashing");
        //body.AddRelativeForce(Vector2.forward * dashLenght, ForceMode2D.Impulse);
        //}
    }

    public void ResetMomentum() {
        Vector2 newVelocity = new Vector2(0, 0);
        body.velocity = newVelocity;
    }

    //========================================================================================

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovement;
        input.OnPlayerRun -= HandleRunning;
        input.OnPlayerJump -= HandleJumping;
        input.OnPlayerDash -= HandleDashing;
    }
}
