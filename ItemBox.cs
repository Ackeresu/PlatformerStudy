using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer newSprite;

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
    private int[] ringList = { 1, 5, 10, 30, 50 };

    private Effects effects;

    private const string PLAYER = "Player";

    private void Awake() {
        newSprite = GetComponentInChildren<SpriteRenderer>();
        UpdateSprite();
        GetRingValue();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == PLAYER) {
            effects = collision.gameObject.GetComponentInChildren<Effects>();

            // Rings
            if (boxType == BoxType.Ring5 || boxType == BoxType.Ring10 || boxType == BoxType.RingRandom) {
                ScoreTracker.Instance.AddRing(ringValue);
            }
            // Shields
            else if (boxType == BoxType.Shield) {
                effects.ActivateShield(false);
            }
            else if (boxType == BoxType.MagneticShield) {
                effects.ActivateShield(true);
            }
            // Invincibility
            else if (boxType == BoxType.Invicibility) {
                StartCoroutine(effects.ActivateInvincibility());
            }
            // Speed Boost
            else if (boxType == BoxType.SpeedBoost) {
                effects.ActivateSpeedBoost();
            }
            Acker.GlobalFunctions.ApplyKnockback.VerticalKnockback(collision.attachedRigidbody, knockback);
            Destroy(gameObject);
        }
    }

    private void UpdateSprite() {
        if (boxType == BoxType.Ring5) {
            newSprite.sprite = sprites[0];
        } else if (boxType == BoxType.Ring10) {
            newSprite.sprite = sprites[1];
        } else if (boxType == BoxType.RingRandom) {
            newSprite.sprite = sprites[2];
        } else if (boxType == BoxType.Shield) {
            newSprite.sprite = sprites[3];
        } else if (boxType == BoxType.MagneticShield) {
            newSprite.sprite = sprites[4];
        } else if (boxType == BoxType.Invicibility) {
            newSprite.sprite = sprites[5];
        } else if (boxType == BoxType.SpeedBoost) {
            newSprite.sprite = sprites[6];
        }
    }

    private void GetRingValue() {
        if (boxType == BoxType.Ring5) {
            ringValue = 5;
        }
        if (boxType == BoxType.Ring10) {
            ringValue = 10;
        }
        if (boxType == BoxType.RingRandom) {
            ringValue = Random.Range(0, ringList.Length);
        }
    }
}
