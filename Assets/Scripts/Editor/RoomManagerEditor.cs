using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for creating and joining room.", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;
        if (GUILayout.Button("Join Exercise Room"))
            roomManager.JoinExerciseRoom();
        else if (GUILayout.Button("Join Experiment Room"))
            roomManager.JoinExperimentRoom();
    }
}