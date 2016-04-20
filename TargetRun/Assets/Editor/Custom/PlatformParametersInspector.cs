using UnityEditor;

[CustomEditor(typeof(ConnectionParameters.PlatformParamaters))]
public class PlatformParametersInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Chance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Platform"));
        serializedObject.ApplyModifiedProperties();
    }
}
