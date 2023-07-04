using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private InputActions input;
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D jumpingCollider;

    public float moveSpeed = 3f;
    public float jumpForce = 2f;
    public int maxAirJumps = 0;
    
    private Rigidbody2D body;
    private Animator anim;
    private float movementX;
    private int availableAirJumps;

    private CheckGroundedState groundedState;
    private bool shouldCheckGround;

    // Animation variables
    private static string SPEED = "Speed";
    private static string IS_JUMPING = "isJumping";


    private void Start() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundedState = GetComponentInChildren<CheckGroundedState>();

        availableAirJumps = maxAirJumps;

        input.OnPlayerMovement += HandleMovement;
        input.OnPlayerJump += HandleJumping;
    }

    private void Update() {
        HandleStuff();

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

    private void HandleStuff() {
        Vector2 movement = new Vector2(movementX, body.velocity.y);
        body.velocity = movement;

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
        if (!Mathf.Approximately(movementX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-movementX) / pScale.x, 1 / pScale.y, 1);
        }

        // Scale for animation
        anim.SetFloat(SPEED, Mathf.Abs(movementX));
        if (!Mathf.Approximately(movementX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(-movementX), 1, 1);
        }
    }

    private void HandleMovement(object sender, Vector2 inputMovement) {
        movementX = inputMovement.x * moveSpeed;
    }

    private void HandleJumping(object sender, EventArgs empty) {
        if (groundedState.GetIsGrounded()) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            standingCollider.enabled = false;
            jumpingCollider.enabled = true;

            anim.SetBool(IS_JUMPING, true);
        }

        if (!groundedState.GetIsGrounded() && availableAirJumps > 0) {
            body.AddForce(Vector2.up * (jumpForce / 2), ForceMode2D.Impulse);
            availableAirJumps -= 1;
        }
    }

    private void OnDestroy() {
        input.OnPlayerMovement -= HandleMovement;
        input.OnPlayerJump -= HandleJumping;
    }
}
