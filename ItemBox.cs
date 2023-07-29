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

    private float knockback = 3.5f;
    private int ringValue;

    private List<int> ringList = new List<int>() { 1, 5, 10, 30, 50 };

    private const string PLAYER = "Player";

    private void Start() {
        if (boxType == BoxType.Ring5) {
            ringValue = 5;
        }
        if (boxType == BoxType.Ring10) {
            ringValue = 10;
        }
        if (boxType == BoxType.RingRandom) {
            ringValue = Random.Range(0, ringList.Count);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            // Rings
            if (boxType == BoxType.Ring5 || boxType == BoxType.Ring10 || boxType == BoxType.RingRandom) {
                Acker.GlobalFunctions.ApplyKnockback.VerticalKnockback(collision.attachedRigidbody, knockback);
                ScoreTracker.Instance.AddRing(ringValue);
            }
            // Shields
            else if (boxType == BoxType.Shield || boxType == BoxType.MagneticShield) {

            }
            // Invincibility
            else if (boxType == BoxType.Invicibility) {

            }
            // Speed Boost
            else if (boxType == BoxType.SpeedBoost) {

            }
            Destroy(gameObject);
        }
    }
}
