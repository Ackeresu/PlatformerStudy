using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Platform))]
[CanEditMultipleObjects]

public class triggerPlatformEditor : Editor {

    SerializedProperty platformType;
    SerializedProperty speed;

    // Moving platform
    SerializedProperty finishPos;

    // Rotating platform
    SerializedProperty rotationPivot;
    SerializedProperty antiClockwise;

    // Falling platform
    SerializedProperty secondsBeforeFall;
    SerializedProperty doesRespawn;

    void OnEnable() {
        platformType = serializedObject.FindProperty("platformType");
        speed = serializedObject.FindProperty("speed");

        // Moving platform
        finishPos = serializedObject.FindProperty("finishPos");

        // Rotating platform
        rotationPivot = serializedObject.FindProperty("rotationPivot");
        antiClockwise = serializedObject.FindProperty("antiClockwise");

        // Falling platform
        secondsBeforeFall = serializedObject.FindProperty("secondsBeforeFall");
        doesRespawn = serializedObject.FindProperty("doesRespawn");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(platformType);

        if (platformType.enumValueIndex == 1) {
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(finishPos);
        }
        if (platformType.enumValueIndex == 2) {
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(rotationPivot);
            EditorGUILayout.PropertyField(antiClockwise);
        }
        if (platformType.enumValueIndex == 3) {
            EditorGUILayout.PropertyField(secondsBeforeFall);
            EditorGUILayout.PropertyField(doesRespawn);
        }
        serializedObject.ApplyModifiedProperties();
    }
}