using UnityEngine;

[CreateAssetMenu(fileName = "TaskPlacingWeight", menuName = "Exercise/Tasks/TaskPlacingWeight")]
public class TaskPlacingWeight : Task
{
    public AttachableObjectType weightType;
    public int target = -1;
}