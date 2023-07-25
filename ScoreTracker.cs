using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour {

    public static ScoreTracker Instance;

    private void Singleton() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private int rings = 0;

    private void Awake() {
        Singleton();
    }

    public void AddRing() {
        rings++;
        Debug.Log($"Rings: {rings}");
    }

    public void PlayerHit() {
        rings = 0;
        Debug.Log($"Rings: {rings}");
    }
}
