using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Spring : MonoBehaviour {

    public enum SpringDirection {
        Up,
        Side,
        DiagonalUp,
        DiagonalDown,
    };
    public float force = 5;

    [SerializeField] bool shouldLockMovement = false;
    [SerializeField] float lockTime = 0.5f;

    private float springAngle;
    private Player player;
    private Rigidbody2D playerBody;

    private const string PLAYER = "Player";

    private void Start() {
        springAngle = transform.rotation.eulerAngles.z;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            player = collision.gameObject.GetComponent<Player>();
            playerBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (springAngle == 0 || springAngle == 180) {
                player.Jump(playerBody.velocity.x, 0, transform.up * force);
            } else {
                player.Jump(0, 0, transform.up * force);
            }

            if (shouldLockMovement) {
                player.LockMovement(lockTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            player = null;
        }
    }
}
