using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ExtraPlayerEffects : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AnimatorController[] animatorControllers;

    private bool isShielded;
    private bool isInvicible;
    private bool isSpeedBoosted;

    private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool GetIsShielded() => isShielded;
    public bool GetIsInvincible() => isInvicible;
    public bool GetIsSpeedBoosted() => isSpeedBoosted;

    private void Awake() {
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    //private void Update() {
    //    CheckActiveEffect();
    //}

    public void ActivateShield(bool isMagnetic) {
        isShielded = true;

        if (!isMagnetic) {
            SetEffect(0);
        } else {
            SetEffect(1);
        }
    }

    public void ActivateInvincibility() {
        isInvicible = true;
        SetEffect(2);
    }

    public void ActivateSpeedBoost() {
        isSpeedBoosted = true;
        player.topSpeed *= 2;
    }

    public void StopShield() {
        spriteRenderer.enabled = false;
        animator.enabled = false;
        isShielded = false;
    }

    private void SetEffect(int index) {
        spriteRenderer.enabled = true;
        animator.enabled = true;
        spriteRenderer.sprite = sprites[index];
        animator.runtimeAnimatorController = animatorControllers[index];
    }
}
