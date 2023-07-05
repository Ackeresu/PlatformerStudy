using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerStep : MonoBehaviour {

    private Player player;
    private bool isPlayerStepOn;

    public bool IsPlayerOn() => isPlayerStepOn;

    private void Gigi() {
        player = GetComponent<Player>();
        //if (player.groundedState) {
        //    isPlayerStepOn = true;
        //}
        
    }
    private void OnCollisionExit2D(Collision2D collision) {
        isPlayerStepOn = false;
    }
}