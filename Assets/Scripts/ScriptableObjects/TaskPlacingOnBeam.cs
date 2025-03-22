using UnityEngine;

[CreateAssetMenu(fileName = "TaskPlacingOnBeam", menuName = "Exercise/Tasks/TaskPlacingOnBeam")]
public class TaskPlacingOnBeam : Task
{
    public AttachableObjectType weightType;
    [Range(0, 1)] public float normalizedBeamPosition;
    [Range(0, 0.5f)] public float normalizedBeamRange;
}