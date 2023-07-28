using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    private Animator animator;

    private Animation anim;

    private const string SPEED = "Speed";
    private const string IS_JUMPING = "isJumping";
    private const string IS_RAISING = "isRaising";
    private const string IS_FALLING = "isFalling";
    private const string IS_CROUCHING = "isCrouching";
    private const string GOT_HIT = "gotHit";

    private void Awake() {
        animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
    }

    public void MoveAnimation(float moveDir) {
        animator.SetFloat(SPEED, Mathf.Abs(moveDir));
    }

    public void JumpAnimation(bool isJumping) {
        animator.SetBool(IS_JUMPING, isJumping);
    }

    public void RaisingAnimation(bool isRaising) {
        animator.SetBool(IS_RAISING, isRaising);
    }

    public void FallingAnimation(bool isFalling) {
        animator.SetBool(IS_FALLING, isFalling);
    }

    public void CrouchingAnimation(float dir, bool isCrouching) {
        //anim["Crouching"].speed *= dir;
        animator.SetBool(IS_CROUCHING, isCrouching);
    }

    public void HitAnimation(bool gotHit) {
        animator.SetBool(GOT_HIT, gotHit);
    }
}
