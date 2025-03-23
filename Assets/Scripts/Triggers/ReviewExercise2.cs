using UnityEngine;

public class ReviewExercise2 : ReviewExercise
{
    [SerializeField] private AttachableContainer[] attachableContainers;

    protected override void ReviewAttachment()
    {
        var taskCount = section.tasks.Count;
        var attachableContainerCount = attachableContainers.Length;
        if (taskCount != attachableContainerCount)
        {
            Debug.LogWarning("Number of task ≠ container");
            return;
        }

        for (int i = 0; i < section.tasks.Count; i++)
        {
            var task = section.tasks[i];
            if (task is TaskPlacingWeight taskPlacingWeight)
            {
                var taskIndex = taskPlacingWeight.target;
                var attachableContainerIndex = attachableContainers[i].containerIndex;
                if (taskIndex != attachableContainerIndex)
                {
                    Debug.LogWarning("Task index ≠ container index");
                    return;
                }

                var taskType = taskPlacingWeight.weightType;
                var countAttachableObject = attachableContainers[i].attachedObjectInsideCollider.Count;
                if (countAttachableObject == 0)
                {
                    if (taskType == AttachableObjectType.EMPTY) continue;

                    TriggerIncorrectAnswer();
                    Debug.Log("Incorrect Answer!");
                    return;
                }

                var attachableObjectType = attachableContainers[i].attachedObjectInsideCollider[0].attachableObjectType;
                if (taskType != attachableObjectType)
                {
                    TriggerIncorrectAnswer();
                    Debug.Log("Incorrect Answer!");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Incorrect task type!");
                return;
            }
        }

        TriggerCorrectAnswer(attachableContainers);
    }
}