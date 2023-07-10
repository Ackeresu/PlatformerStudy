using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Platform))]
[RequireComponent(typeof(Rigidbody2D))]

public class FallingPlatform : MonoBehaviour {

    private Platform platform;
    public bool doesRespawn = true;
    public float secBeforeFall = 1;
    public float secBeforeDespawn = 3;

    private bool isFalling = false;
    private Rigidbody2D body;
    private Platform.PlatformType originalPlatformType;

    private void Reset() {
        body = GetComponent<Rigidbody2D>();

        body.gravityScale = 0;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Start() {
        platform = GetComponentInParent<Platform>();
        body = GetComponent<Rigidbody2D>();

        originalPlatformType = platform.platformType;
    }

    private void Update() {
        if (platform.playerIsOn && !isFalling) {
                StartCoroutine(MakePlatformFall());
            }
            //if (platform.platformType == Platform.PlatformType.Moving && platform.stopAtEnd) {
            //    Debug.Log("bubu");
            //    if (platform.transform.position == platform.finishPos) {
            //        StartCoroutine(MakePlatformFall());
            //    }
            //}
        //}
    }

    private IEnumerator MakePlatformFall() {
        isFalling = true;

        if (platform.playerIsOn) {
            yield return new WaitForSecondsRealtime(secBeforeFall);

            platform.platformType = Platform.PlatformType.Stationary;
            body.constraints = RigidbodyConstraints2D.None;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.gravityScale = 1;

            yield return new WaitForSecondsRealtime(secBeforeDespawn);

            if (doesRespawn) {
                body.gravityScale = 0;
                body.constraints = RigidbodyConstraints2D.FreezeAll;
                platform.ResetPlatform(originalPlatformType);
            } else {
                Destroy(platform.gameObject);
            }
        }
        isFalling = false;
    }
}
