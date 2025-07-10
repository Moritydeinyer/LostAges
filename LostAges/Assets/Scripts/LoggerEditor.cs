#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Logger))]
public class LoggerEditor : Editor
{
    string debugMessage = "";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        debugMessage = EditorGUILayout.TextField("Debug Message", debugMessage);

        if (GUILayout.Button("Log Message"))
        {
            Debug.Log("[Inspector Log] " + debugMessage);
        }
    }
}
#endif
