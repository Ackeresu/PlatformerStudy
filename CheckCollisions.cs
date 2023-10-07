using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CheckCollisions : MonoBehaviour {

    public bool playerCollision;
    public bool obstacleCollision;

    private const string PLAYER = "Player";
    private const string GROUND = "Ground";

    public bool GetPlayerCollision() => playerCollision;
    public bool GetObstacleCollision() => obstacleCollision;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            playerCollision = true;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == GROUND) {
            obstacleCollision = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            playerCollision = false;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == GROUND) {
            obstacleCollision = false;
        }
    }
}
