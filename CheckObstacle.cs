using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacle : MonoBehaviour {

    public bool obstacleCollision;

    private const string GROUND = "Ground";

    public bool GetObstacleCollision() => obstacleCollision;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == GROUND) {
            obstacleCollision = true;
        }  
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == GROUND) {
            obstacleCollision = false;
        }
    }
}
