using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckGroundedState : MonoBehaviour {

    public bool isGrounded;

    public bool GetIsGrounded() => isGrounded;
 
    private void OnCollisionEnter2D(Collision2D collision) {
        isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}