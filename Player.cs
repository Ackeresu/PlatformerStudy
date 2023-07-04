using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private InputActions input;
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D jumpingCollider;

    public float moveSpeed = 2f;
    public float runSpeed = 3f;
    public float jumpHeight = 4f;
    public float airJumpHeight = 3f;
    public int maxAirJumps = 1;
    public float dashLenght = 2f;
    
    private Rigidbody2D body;
    private Animator anim;

    private Vector2 playerMovement;
    private float movementInputX;
    //private float movementInputY;
    private int availableAirJumps;

    private CheckGroundedState groundedState;
    private bool shouldCheckGround;
    private bool isRunning;

    // Animation variables
    private static string SPEED = "Speed";
    private static string IS_JUMPING = "isJumping";
    //private static string IS_CROUCHING = "isCrouching";

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundedState = GetComponentInChildren<CheckGroundedState>();

        availableAirJumps = maxAirJumps;

        input.OnPlayerMovement += HandleMovement;
        input.OnPlayerRun += HandleRunning;
        input.OnPlayerJump += HandleJumping;
        input.OnPlayerDash += HandleDashing;
    }

    private void Update() {
        UpdateMovement();
        HandleStuff();
        GroundCheck();
    }

    private void UpdateMovement() {
        if (!isRunning) {
            playerMovement = new Vector2(movementInputX * moveSpeed, body.velocity.y);
        } else {
            playerMovement = new Vector2(movementInputX * runSpeed, body.velocity.y);
        }
        body.velocity = playerMovement;

        //body.gravityScale = (groundedState && Mathf.Approximately(movementX, 0)) ? 0 : 1;
    }

    private void HandleStuff() {
        MovingPlatform platform = null;
        if (groundedState != null) {
            platform = groundedState.GetComponent<MovingPlatform>();
        }
        if (platform != null) {
            transform.parent = platform.transform;
        } else {
            transform.parent = null;
        }

        // Player scale
        Vector3 pScale = Vector3.one;
        if (platform != null) {
            pScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x) / pScale.x, 1 / pScale.y, 1);
        }

        // Animations
        anim.SetFloat(SPEED, Mathf.Abs(playerMovement.x));

        if (!Mathf.Approximately(playerMovement.x, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-playerMovement.x), 1, 1);
        }
        //if (!Mathf.Approximately(0, movementY)) {
        //    anim.SetBool(IS_CROUCHING, true);
        //} else {
        //    anim.SetBool(IS_CROUCHING, false);
        //}
    }

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

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovement;
        input.OnPlayerRun -= HandleRunning;
        input.OnPlayerJump -= HandleJumping;
        input.OnPlayerDash -= HandleDashing;
    }
}
