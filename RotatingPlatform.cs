using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RotatingPlatform : MonoBehaviour {

    public float speed = 1f;
    public bool antiClockwise = false;
    public Vector3 rotationPivot = Vector3.zero;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, rotationPivot);
        Gizmos.DrawSphere(rotationPivot, 0.05f);
    }

    private void Update() {
        RotatePlatform();
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