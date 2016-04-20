using UnityEditor;

[CustomEditor(typeof(ConnectionParameters))]
public class ConnectionParametersInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HostExitConnection"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Platforms"));
        serializedObject.ApplyModifiedProperties();
    }
}
