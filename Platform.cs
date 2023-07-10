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

#if isFalling
[RequireComponent(typeof(Rigidbody2D))]
#endif
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]

public class Platform : MonoBehaviour {

    public enum PlatformType {
        Stationary,
        Moving,
        Rotating,
    };
    public PlatformType platformType;
    public bool playerIsOn;

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
    private bool goBackToStart = false;
    private List<Vector3> posList = new List<Vector3>();
    private Vector3[] posArray;

    // Rotating platform
    public Vector3 rotationPivot = Vector3.zero;
    public bool antiClockwise = false;

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
            if (intermediatePos.Count == 0) {
                Gizmos.DrawLine(transform.position, finishPos);
            } else {
                if (startPos == Vector3.zero) {
                    startPos = transform.position;
                }
                posList.Clear();
                posList.Add(startPos);
                posList.AddRange(intermediatePos);
                posList.Add(finishPos);
                posArray = posList.ToArray();

                if (circleBetweenPos) {
                    Gizmos.DrawLineStrip(posArray, true);
                } else {
                    Gizmos.DrawLineStrip(posArray, false);
                }
            }
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
        if (intermediatePos.Count == 0) {
            oldPos = startPos;
            newPos = finishPos;

            if (stopAtEnd && trackPercent > 1) {
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
        } else if (currentStop > 0 && currentStop < intermediatePos.Count) {
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
        } else if (currentStop == intermediatePos.Count && !goBackToStart) {
            oldPos = intermediatePos[currentStop - 1];
            newPos = finishPos;

            if (stopAtEnd && trackPercent > 1) {
                trackPercent = 1;
            } else {
                UpdatePlatformPosition();

                if (direction == 1 && trackPercent > 1 && !circleBetweenPos) {
                    direction *= -1;
                } else if (direction == 1 && trackPercent > 1 && circleBetweenPos) {
                    trackPercent = 0;
                    goBackToStart = true;
                }
                if (direction == -1 && trackPercent < 0) {
                    trackPercent = 1;
                    currentStop--;
                }
            }
        } else if (currentStop == intermediatePos.Count && goBackToStart) {
            oldPos = finishPos;
            newPos = startPos;

            UpdatePlatformPosition();

            if (trackPercent > 1) {
                trackPercent = 0;
                currentStop = 0;
                goBackToStart = false;
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

    public void SetPlayerOn(Transform player, bool isOn) {
        if (isOn) {
            player.SetParent(transform);
            playerIsOn = true;
        } else {
            player.SetParent(null);
            playerIsOn = false;
        }
    }

    public void ResetPlatform(PlatformType type) {
        platformType = type;
        transform.position = startPos;
        trackPercent = 0;
        direction = 1;
        currentStop = 0;
    }
}