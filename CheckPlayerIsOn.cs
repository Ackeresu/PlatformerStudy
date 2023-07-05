using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPlayerIsOn : MonoBehaviour {

    [SerializeField] private Player player;
    public bool playerIsOn;

    public bool PlayerIsOn() => playerIsOn;

    private void OnCollisionStay2D(Collision2D collision) {
        if (player.platform != null) {
            playerIsOn = true;
        } else {
            playerIsOn = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
            playerIsOn = false;
    }
}
