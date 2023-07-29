using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

    public enum BoxType {
        Ring5,
        Ring10,
        RingRandom,
        Shield,
        MagneticShield,
        Invicibility,
        SpeedBoost,
    };
    public BoxType boxType;

    private float knockback = 3f;
    private int ringValue;

    private const string PLAYER = "Player";

    private void Start() {
        if (boxType == BoxType.Ring5) {
            ringValue = 5;
        }
        if (boxType == BoxType.Ring10) {
            ringValue = 10;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            if (boxType == BoxType.Ring5 || boxType == BoxType.Ring10) {
                Acker.GlobalFunctions.ApplyKnockback.VerticalKnockback(collision.attachedRigidbody, knockback);
                ScoreTracker.Instance.AddRing(ringValue);
                Destroy(gameObject);
            }
        }
    }
}
