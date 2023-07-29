using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Acker.GlobalFunctions {

    public class ApplyKnockback {
        public static void VerticalKnockback(Rigidbody2D body, float minKnockback) {
            float velY;

            if (Mathf.Abs(body.velocity.y) < minKnockback) {
                velY = minKnockback;
            } else {
                velY = Mathf.Abs(body.velocity.y);
            }
            Vector2 knockback = new Vector2(body.velocity.x, velY);

            body.AddForce(knockback, ForceMode2D.Impulse);
        }
    }
}
