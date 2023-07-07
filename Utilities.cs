using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace Acker.Utility {
    public class ActionOverTime {
        /// <summary>
        /// Change an object WORLD position with a Vector3.lerp
        /// </summary>
        /// <param name="objectToMove">The Transform to move</param>
        /// <param name="startPos">Start position</param>
        /// <param name="endPos">End position</param>
        /// <param name="duration">The operation Duration</param>
        /// <returns></returns>
        public static IEnumerator LerpObjectPositionOverTime(Transform objectToMove, UnityEngine.Vector3 startPos, UnityEngine.Vector3 endPos,
            float duration) {
            float time = 0.0f;
            while (time <= 1.0f) {
                time += Time.deltaTime / duration;
                objectToMove.position = UnityEngine.Vector3.Lerp(startPos, endPos, time);
                yield return null;
            }

            yield return null;
        }

        /// <summary>
        /// Change an object WORLD position with a Vector3.lerp with an UnityAction called at the end of the operation
        /// </summary>
        /// <param name="objectToMove">The Transform to move</param>
        /// <param name="startPos">Start position</param>
        /// <param name="endPos">End position</param>
        /// <param name="duration">The operation Duration</param>
        /// <param name="afterLerpAction">The UnityAction called at the end of the operation</param>
        /// <returns></returns>
        public static IEnumerator LerpObjectWorldPositionOverTime(Transform objectToMove, UnityEngine.Vector3 startPos,
            UnityEngine.Vector3 endPos, float duration, UnityAction afterLerpAction) {
            float time = 0.0f;
            while (time <= 1.0f) {
                time += Time.deltaTime / duration;
                objectToMove.position = UnityEngine.Vector3.Lerp(startPos, endPos, time);
                yield return null;
            }

            afterLerpAction?.Invoke();
            yield return null;
        }

        /// <summary>
        /// Change an object LOCAL position with a Vector3.lerp with an UnityAction called at the end of the operation
        /// </summary>
        /// <param name="objectToMove">The Transform to move</param>
        /// <param name="startPos">Start position</param>
        /// <param name="endPos">End position</param>
        /// <param name="duration">The operation Duration</param>
        /// <param name="afterLerpAction">The UnityAction called at the end of the operation</param>
        /// <returns></returns>
        public static IEnumerator LerpObjectLocalPositionOverTime(Transform objectToMove, UnityEngine.Vector3 startPos,
            UnityEngine.Vector3 endPos, float duration, UnityAction afterLerpAction) {
            float time = 0.0f;
            while (time <= 1.0f) {
                time += Time.deltaTime / duration;
                objectToMove.localPosition = UnityEngine.Vector3.Lerp(startPos, endPos, time);
                yield return null;
            }

            afterLerpAction?.Invoke();
            yield return null;
        }
    }
}