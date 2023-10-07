using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Effects : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AnimatorController[] animatorControllers;

    public float invincibilityDuration;
    public float speedBoostValue;

    private bool isShielded;
    private bool isInvincible;
    private bool isSpeedBoosted;

    private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool GetIsShielded() => isShielded;
    public bool GetIsInvincible() => isInvincible;
    public bool GetIsSpeedBoosted() => isSpeedBoosted;

    private void Awake() {
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    public void ActivateShield(bool isMagnetic) {
        isShielded = true;

        if (!isMagnetic) {
            SetEffect(0);
        } else {
            SetEffect(1);
        }
    }

    // =================================================== DA FIXARE =============================================================
    public IEnumerator ActivateInvincibility() {
        isInvincible = true;
        SetEffect(2);

        yield return new WaitForSeconds(invincibilityDuration);

        spriteRenderer.enabled = false;
        animator.enabled = false;

        isInvincible = false;

        ////Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //
        ////yield on a new YieldInstruction that waits for 5 seconds.
        //yield return new WaitForSeconds(5);
        //
        ////After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    // =================================================== DA FIXARE =============================================================

    public void ActivateSpeedBoost() {
        isSpeedBoosted = true;
        player.topSpeed += speedBoostValue;
    }

    public void StopShield() {
        isShielded = false;
        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    private void SetEffect(int index) {
        spriteRenderer.enabled = true;
        animator.enabled = true;
        spriteRenderer.sprite = sprites[index];
        animator.runtimeAnimatorController = animatorControllers[index];
    }
}
