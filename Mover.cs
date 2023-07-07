using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Mover : MonoBehaviour {

    public List<Transform> targets;

    private Vector3 startPos;
    private Vector3 actualTarget;
    private int lastTarget;

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        for (int i = 0; i < targets.Count; i++) {
            Gizmos.DrawSphere(targets[i].position, 0.05f);
        }
    }

    private void Start() {
        startPos = targets[0].position;
        transform.position = startPos;
        actualTarget = targets[1].position;

        lastTarget = 1;

        Moving();
    }

    void Moving() {
        StartCoroutine(Acker.Utility.ActionOverTime.LerpObjectWorldPositionOverTime(transform, startPos, actualTarget, 2, ChangeTarget));
    }

    void ChangeTarget() {
        startPos = targets[lastTarget].position;
        lastTarget++;

        if (lastTarget < targets.Count) {
            actualTarget = targets[lastTarget].position;
        } else {
            actualTarget = targets[0].position;
            lastTarget = 0;
        }

        Moving();
    }
}
