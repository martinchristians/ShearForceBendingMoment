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

        for (int i = 0; i < section.tasks.Count; i++)
        {
            var task = section.tasks[i];

            if (task is TaskPlacingSymbol taskPlacingSymbol)
            {
                var taskIndex = taskPlacingSymbol.target;
                var taskType = taskPlacingSymbol.symbolType;

                var countAttachableObject = attachableContainers[i].attachedObjectInsideCollider.Count;
                if (countAttachableObject == 0)
                {
                    TriggerIncorrectAnswer();
                    Debug.Log("Empty target");
                    return;
                }

                var attachableContainerIndex = attachableContainers[i].containerIndex;
                var attachableObjectType = attachableContainers[i].attachedObjectInsideCollider[0].attachableObjectType;

                if (taskIndex != attachableContainerIndex || taskType != attachableObjectType)
                {
                    TriggerIncorrectAnswer();
                    Debug.Log("Incorrect Result!");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Incorrect task type!!");
                return;
            }
        }

        TriggerCorrectAnswer();
    }
}