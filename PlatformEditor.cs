using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Platform))]
[CanEditMultipleObjects]

public class PlatformEditor : Editor {

    SerializedProperty platformType;
    SerializedProperty speed;

    // Moving platform
    SerializedProperty intermediatePos;
    SerializedProperty finishPos;

    // Rotating platform
    SerializedProperty rotationPivot;
    SerializedProperty antiClockwise;

    void OnEnable() {
        platformType = serializedObject.FindProperty("platformType");
        speed = serializedObject.FindProperty("speed");

        // Moving platform
        intermediatePos = serializedObject.FindProperty("intermediatePos");
        finishPos = serializedObject.FindProperty("finishPos");

        // Rotating platform
        rotationPivot = serializedObject.FindProperty("rotationPivot");
        antiClockwise = serializedObject.FindProperty("antiClockwise");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(platformType);

        if (platformType.enumValueIndex == 1) {
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(intermediatePos);
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