using UnityEngine;

[CreateAssetMenu(fileName = "TaskPlacingSymbol", menuName = "Exercise/Tasks/TaskPlacingSymbol")]
public class TaskPlacingSymbol : Task
{
    public AttachableObjectType symbolType;
    public int target = -1;
}