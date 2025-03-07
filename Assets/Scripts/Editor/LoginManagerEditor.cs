using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoginManager))]
public class LoginManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for connecting to Photon servers.", MessageType.Info);

        LoginManager loginManager = (LoginManager)target;
        if (GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectAnonymously();
        }
    }
}