using UnityEngine;

[CreateAssetMenu(fileName = "TaskPlacingOnBeam", menuName = "Exercise/Tasks/TaskPlacingOnBeam")]
public class TaskPlacingOnBeam : Task
{
    public AttachableObjectType weightType;
    [Range(0, 1)] public float normalizedBeamPosition;
    [Range(0, 0.5f)] public float normalizedBeamRange;

    public float WeightForce
    {
        get
        {
            return weightType switch
            {
                AttachableObjectType.SPOTLIGHT => AttachableObjectConstants.SPOTLIGHT_FORCE,
                AttachableObjectType.MOVING_HEAD => AttachableObjectConstants.MOVING_HEAD_FORCE,
                AttachableObjectType.SPEAKER => AttachableObjectConstants.SPEAKER_FORCE,
                _ => 0f
            };
        }
    }
}