using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(TriggerPlatform))]
[CanEditMultipleObjects]

public class TriggerPlatformEditor : Editor {

    SerializedProperty platformType;
    SerializedProperty speed;

    // Falling platform
    SerializedProperty secondsBeforeFall;
    SerializedProperty doesRespawn;

    // Moving platform
    SerializedProperty finishPos;

    // Rotating platform
    SerializedProperty rotationPivot;
    SerializedProperty antiClockwise;

    void OnEnable() {
        platformType = serializedObject.FindProperty("platformType");
        speed = serializedObject.FindProperty("speed");

        // Falling platform
        secondsBeforeFall = serializedObject.FindProperty("secondsBeforeFall");
        doesRespawn = serializedObject.FindProperty("doesRespawn");

        // Moving platform
        finishPos = serializedObject.FindProperty("finishPos");

        // Rotating platform
        rotationPivot = serializedObject.FindProperty("rotationPivot");
        antiClockwise = serializedObject.FindProperty("antiClockwise");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(platformType);

        if (platformType.enumValueIndex == 0) {
            EditorGUILayout.PropertyField(secondsBeforeFall);
            EditorGUILayout.PropertyField(doesRespawn);
        }
        if (platformType.enumValueIndex == 1) {
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(finishPos);
        }
        if (platformType.enumValueIndex == 2) {
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(rotationPivot);
            EditorGUILayout.PropertyField(antiClockwise);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}