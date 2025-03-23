using UnityEngine;

public class ReviewExercise1 : ReviewExercise
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

        for (int i = 0; i < taskCount; i++)
        {
            var task = section.tasks[i];
            if (task is TaskPlacingSymbol taskPlacingSymbol)
            {
                var taskIndex = taskPlacingSymbol.target;
                var attachableContainerIndex = attachableContainers[i].containerIndex;
                if (taskIndex != attachableContainerIndex)
                {
                    Debug.LogWarning("Task index ≠ container index");
                    return;
                }

                //All attachable boxes must be occupied
                var countAttachableObject = attachableContainers[i].attachedObjectInsideCollider.Count;
                if (countAttachableObject == 0)
                {
                    Debug.Log("Empty attachable box!");
                    return;
                }

                var taskType = taskPlacingSymbol.symbolType;
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