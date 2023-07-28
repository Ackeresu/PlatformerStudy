using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed = 1f;
    public float horizontalKnockback = 2f;
    public float verticalKnockback = 3f;

    private int direction = 1;
    private Vector3 movement;

    private CheckObstacle checkObstacle;

    private void Start() {
        checkObstacle = GetComponentInChildren<CheckObstacle>();
    }

    private void Update() {
        UpdateMovement();
    }

    private void UpdateMovement() {
        movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
        transform.position += movement;

        if (checkObstacle.GetObstacleCollision()) {
            direction *= -1;
            transform.localScale = new Vector3(1 * direction, 1, 1);
        }
    }
}
