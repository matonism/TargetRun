using UnityEditor;

[CustomEditor(typeof(DistanceLimiting))]
public class DistanceLimitingInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distance"));
        serializedObject.ApplyModifiedProperties();
    }
}
