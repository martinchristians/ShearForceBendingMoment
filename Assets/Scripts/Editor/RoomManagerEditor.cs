using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("These buttons are used for testing in desktop modus to create and join room.", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;
        if (GUILayout.Button("Join Exercise1 Room"))
            roomManager.JoinExercise1Room();
        else if (GUILayout.Button("Join Experiment Room"))
            roomManager.JoinExperimentRoom();
    }
}