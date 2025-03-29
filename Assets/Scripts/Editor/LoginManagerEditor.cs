using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoginManager))]
public class LoginManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("These buttons are used for testing in desktop modus to connect to Photon servers.",
            MessageType.Info);

        LoginManager loginManager = (LoginManager)target;
        if (GUILayout.Button("Connect as 1")) loginManager.ConnectAndLoadLobby();
    }
}