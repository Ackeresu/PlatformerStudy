using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {

    public int ringValue = 1;

    private const string PLAYER = "Player";

    private void OnTriggerEnter2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            ScoreTracker.Instance.AddRing();
            Destroy(this.gameObject);
        }
    }
}
