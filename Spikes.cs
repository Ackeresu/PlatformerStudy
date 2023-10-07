using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    [SerializeField] private Rigidbody2D body;

    public float horizontalKnockback = 2f;
    public float verticalKnockback = 3f;

    private CheckCollisions checkCollisions;
    private Player player;
    private Rigidbody2D playerBody;
    private Effects effects;

    private void Awake() {
        checkCollisions = GetComponentInChildren<CheckCollisions>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        

    }
}
