using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
    public bool stopAtEnd = false;
    public bool circleBetweenPos = false;
    public List<Vector3> intermediatePos;
    public Vector3 finishPos = Vector3.zero;

    private Vector3 startPos;
    private Vector3 oldPos;
    private Vector3 newPos;
    private float posX, posY;
    private float trackPercent = 0;
    private int direction = 1;
    private int currentStop = 0;
    //private List<Vector3> posList;
    //private Vector3[] posArray;

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
        //   if (intermediatePos.Count > 0) {
        //        posList.Add(transform.position);
        //        posList.AddRange(intermediatePos);
        //        posList.Add(finishPos);
        //        
        //        posArray = posList.ToArray();
        //        Gizmos.DrawLineStrip(posArray, false);
        //        posList.Clear();
        //    } else {
               Gizmos.DrawLine(transform.position, finishPos);
        //   }
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

        // There are not intermediate positions
        if (intermediatePos.Count <= 0) {
            oldPos = startPos;
            newPos = finishPos;

            if (stopAtEnd && trackPercent >= 1) {
                trackPercent = 1;
            } else {
                UpdatePlatformPosition();

                if ((direction == 1 && trackPercent > 1) || (direction == -1 && trackPercent < 0)) {
                    direction *= -1;
                }
            }
        }
        // There are intermediate positions
        else if (currentStop == 0) {
            oldPos = startPos;
            newPos = intermediatePos[currentStop];

            UpdatePlatformPosition();

            if (direction == 1 && trackPercent > 1) {
                trackPercent = 0;
                currentStop++;
            }
            if (direction == -1 && trackPercent < 0) {
                direction *= -1;
            }
        }
        else if (currentStop > 0 && currentStop < intermediatePos.Count) {
            oldPos = intermediatePos[currentStop - 1];
            newPos = intermediatePos[currentStop];

            UpdatePlatformPosition();

            if (direction == 1 && trackPercent > 1) {
                trackPercent = 0;
                currentStop++;
            }
            if (direction == -1 && trackPercent < 0) {
                trackPercent = 1;
                currentStop--;
            }
        }
        else if (currentStop == intermediatePos.Count) {
            oldPos = intermediatePos[currentStop - 1];
            newPos = finishPos;

            if (stopAtEnd && trackPercent >= 1) {
                trackPercent = 1;
            } else {
                UpdatePlatformPosition();

                if (direction == 1 && trackPercent > 1) {
                    direction *= -1;
                }
                if (direction == -1 && trackPercent < 0) {
                    trackPercent = 1;
                    currentStop--;
                }
            }
        }
    }

    private void UpdatePlatformPosition() {
        posX = (newPos.x - oldPos.x) * trackPercent + oldPos.x;
        posY = (newPos.y - oldPos.y) * trackPercent + oldPos.y;

        transform.position = new Vector3(posX, posY, startPos.z);
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