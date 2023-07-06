using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(CheckPlayerIsOn))]

public class TriggerPlatform : MonoBehaviour {

    public enum PlatformType {
        Falling,
        Moving,
        Rotating,
    };
    public PlatformType platformType;

    // Shared
    public float speed = 1;

    // Falling platform
    public float secondsBeforeFall = 1;
    public bool doesRespawn = false;

    // Moving platform
    public Vector3 finishPos = Vector3.zero;

    private Vector3 startPos;
    private float trackPercent = 0;
    private int direction = 1;

    // Rotating platform
    public Vector3 rotationPivot = Vector3.zero;
    public bool antiClockwise = false;

    // For settings purposes
    private BoxCollider2D box;
    private PlatformEffector2D effector;
    private CheckPlayerIsOn checkPlayerIsOn;

    private void Reset() {
        box = GetComponent<BoxCollider2D>();
        effector = GetComponent<PlatformEffector2D>();

        box.usedByEffector = true;
        effector.surfaceArc = 170;
    }

    private void Start() {
        startPos = transform.position;
        checkPlayerIsOn = GetComponent<CheckPlayerIsOn>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        if (platformType == PlatformType.Moving) {
            Gizmos.DrawLine(transform.position, finishPos);
        }
        if (platformType == PlatformType.Rotating) {
            Gizmos.DrawLine(transform.position, rotationPivot);
            Gizmos.DrawSphere(rotationPivot, 0.05f);
        }
    }

    private void Update() {
        if (platformType == PlatformType.Falling) {
            FallingPlatform();
        }
        if (platformType == PlatformType.Moving) {
            MovePlatform();
        }
        if (platformType == PlatformType.Rotating) {
            RotatePlatform();
        }
    }

    private void FallingPlatform() {
        if (checkPlayerIsOn.playerIsOn) {
            float timer = secondsBeforeFall * Time.deltaTime;

            Debug.Log(timer);

            if (timer > secondsBeforeFall) {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
                transform.position = newPos;
            }
        }
    }

    private void MovePlatform() {
        if (checkPlayerIsOn.playerIsOn) {
            trackPercent += direction * speed * Time.deltaTime;
            float x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            float y = (finishPos.y - startPos.y) * trackPercent + startPos.y;

            transform.position = new Vector3(x, y, startPos.z);

            if ((direction == 1 && trackPercent > 1) || (direction == -1 && trackPercent < 0)) {
                direction *= -1;
            }
        }
    }

    private void RotatePlatform() {
        if (!antiClockwise) {
            transform.RotateAround(rotationPivot, Vector3.forward, speed * -90 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.RotateAround(rotationPivot, Vector3.forward, speed * 90 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}