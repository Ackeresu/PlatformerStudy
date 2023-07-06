using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]

public class Platform : MonoBehaviour {

    public enum PlatformType {
        Stationary,
        Moving,
        Rotating,
    };
    public PlatformType platformType;

    // Shared
    public float speed = 1;

    // Moving platform
    public Vector3[] intermediatePos;
    public Vector3 finishPos = Vector3.zero;

    private Vector3 startPos;
    private float trackPercent = 0;
    private int direction = 1;
    private int currentStop = 0;

    // Rotating platform
    public Vector3 rotationPivot = Vector3.zero;
    public bool antiClockwise = false;

    // Falling platform
    public float secondsBeforeFall = 1;
    public bool doesRespawn = false;

    // For settings purposes
    private BoxCollider2D box;
    private PlatformEffector2D effector;

    private void Reset() {
        box = GetComponent<BoxCollider2D>();
        effector = GetComponent<PlatformEffector2D>();

        box.usedByEffector = true;
        effector.surfaceArc = 170;
    }

    private void Start() {
        startPos = transform.position;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        if (platformType == PlatformType.Moving) {
            //if (intermediatePos.Length > 0) {
            //    for (var i = 0; i < intermediatePos.Length; i++) {
            //        Debug.Log(i);
            //        if (i == 0) {
            //            Gizmos.DrawLine(transform.position, intermediatePos[i]);
            //        }
            //        else if (i > 0 && i < intermediatePos.Length) {
            //            Gizmos.DrawLine(intermediatePos[i], intermediatePos[i++]);
            //        }
            //        else if (i == intermediatePos.Length) {
            //            Gizmos.DrawLine(intermediatePos[i], finishPos);
            //        }
            //    }
            //} else {
                Gizmos.DrawLine(transform.position, finishPos);
            //}
        }
        if (platformType == PlatformType.Rotating) {
            Gizmos.DrawLine(transform.position, rotationPivot);
            Gizmos.DrawSphere(rotationPivot, 0.05f);
        }
    }

    private void Update() {
        if (platformType == PlatformType.Moving) {
            MovePlatform();
        }
        if (platformType == PlatformType.Rotating) {
            RotatePlatform();
        }
    }

    private void MovePlatform() {
        trackPercent += direction * speed * Time.deltaTime;
        float x, y;
        Vector3 oldPos;
        Vector3 newPos;

        if (intermediatePos.Length > 0) {
            if (currentStop == 0) {
                oldPos = startPos;
                newPos = intermediatePos[currentStop];

                x = (newPos.x - oldPos.x) * trackPercent + oldPos.x;
                y = (newPos.y - oldPos.y) * trackPercent + oldPos.y;

                transform.position = new Vector3(x, y, startPos.z);

                if (direction == 1 && trackPercent > 1) {
                    trackPercent = 0;
                    currentStop++;
                }
                if (direction == -1 && trackPercent < 0) {
                    direction *= -1;
                }
            }

            else if (currentStop > 0 && currentStop < intermediatePos.Length) {
                oldPos = intermediatePos[currentStop - 1];
                newPos = intermediatePos[currentStop];

                x = (newPos.x - oldPos.x) * trackPercent + oldPos.x;
                y = (newPos.y - oldPos.y) * trackPercent + oldPos.y;

                transform.position = new Vector3(x, y, startPos.z);
            
                if (direction == 1 && trackPercent > 1) {
                    trackPercent = 0;
                    currentStop++;
                }
                if (direction == -1 && trackPercent < 0) {
                    trackPercent = 1;
                    currentStop--;
                }
            }

            else if (currentStop == intermediatePos.Length) {
                oldPos = intermediatePos[currentStop - 1];
                newPos = finishPos;

                x = (newPos.x - oldPos.x) * trackPercent + oldPos.x;
                y = (newPos.y - oldPos.y) * trackPercent + oldPos.y;

                transform.position = new Vector3(x, y, startPos.z);

                if (direction == 1 && trackPercent > 1) {
                    direction *= -1;
                }
                if (direction == -1 && trackPercent < 0) {
                    trackPercent = 1;
                    currentStop--;
                }
            }
        } else {
            x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            y = (finishPos.y - startPos.y) * trackPercent + startPos.y;

            transform.position = new Vector3(x, y, startPos.z);

            if ((direction == 1 && trackPercent > 1) || (direction == -1 && trackPercent < 0)) {
                direction *= -1;
            }
        }
    }

    private void UpdatePlatformPosition() {

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