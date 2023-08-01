using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum EnemyType {
        Ladybug,
    };
    public EnemyType enemyType;

    public float moveSpeed = 1f;
    public float horizontalKnockback = 2f;
    public float verticalKnockback = 3f;

    private int direction = 1;
    private float knockback = 3.5f;
    private Vector3 movement;

    private CheckObstacle checkObstacle;
    private Player player;
    private Rigidbody2D playerBody;
    private ExtraPlayerEffects effects;

    private const string PLAYER = "Player";

    private void Awake() {
        checkObstacle = GetComponentInChildren<CheckObstacle>();
    }

    private void Update() {
        UpdateMovement();
    }

    //========================================================================================

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
    //        PlayerCheck(collision.gameObject);
    //    }
    //}
    private void OnCollisionStay2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            PlayerCheck(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            player = null;
        }
    }

    //========================================================================================

    private void UpdateMovement() {
        movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
        transform.position += movement;

        if (checkObstacle.GetObstacleCollision()) {
            direction *= -1;
            transform.localScale = new Vector3(1 * direction, 1, 1);
        }
    }

    private void PlayerCheck(GameObject playerOBJ) {
        player = playerOBJ.gameObject.GetComponent<Player>();
        playerBody = playerOBJ.gameObject.GetComponent<Rigidbody2D>();
        effects = playerOBJ.gameObject.GetComponentInChildren<ExtraPlayerEffects>();

        if (player.GetIsHit()) {
            return;
        }
        if (effects.GetIsInvincible()) {
            Destroy(gameObject);
        }
        else if (player.GetIsJumping()) {
            Acker.GlobalFunctions.ApplyKnockback.VerticalKnockback(playerBody, knockback);
            Destroy(gameObject);
        }
        else if (!player.GetIsJumping()) {
            PlayerHit();
        }
    }

    private void PlayerHit() {
        player.ResetMomentum();

        Vector2 knockback = new Vector2(player.transform.localScale.x * horizontalKnockback, verticalKnockback);
        playerBody.AddForce(knockback, ForceMode2D.Impulse);

        if (player.GetIsHit()) {
            return;
        } else {
            StartCoroutine(player.LockMovementUntilGrounded());
            if (effects.GetIsShielded()) {
                effects.StopShield();
            } else {
                ScoreTracker.Instance.PlayerHit();
            }
        }
    }
}
