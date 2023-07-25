using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundChecker : MonoBehaviour {

    public bool isGrounded;

    private Rigidbody2D playerBody;
    public Platform lastPlatform;

    private Collider2D groundCollider;

    private string PLATFORM = "Platform";

    public bool GetIsGrounded() => isGrounded;

    private void Awake() {
        playerBody = GetComponentInParent<Rigidbody2D>();
        groundCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        // Disable the groundChecker collider if the player is going up
        if (playerBody.velocity.y <= 0.1) {
            if (!groundCollider.enabled) {
                groundCollider.enabled = true;
            }
        } else {
            if (groundCollider.enabled) {
                groundCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        isGrounded = true;

        if (LayerMask.LayerToName(collision.gameObject.layer) == PLATFORM) {
            lastPlatform = collision.gameObject.GetComponent<Platform>();
            lastPlatform.SetPlayerOn(transform.parent, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isGrounded = false;

        if (LayerMask.LayerToName(collision.gameObject.layer) == PLATFORM) {
            if (collision.gameObject.GetComponent<Platform>() == lastPlatform) {
                lastPlatform.SetPlayerOn(transform.parent, false);
                lastPlatform = null;
            }
        }
    }
}