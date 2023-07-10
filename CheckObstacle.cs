using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacle : MonoBehaviour {

    public bool obstacleCollision;

    public bool GetObstacleCollision() => obstacleCollision;

    private void OnCollisionEnter2D(Collision2D collision) {
        obstacleCollision = true;
    }
    private void OnCollisionExit2D(Collision2D collision) {
        obstacleCollision = false;
    }
}
