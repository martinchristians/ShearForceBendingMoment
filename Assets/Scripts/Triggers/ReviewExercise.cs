using System.Collections.Generic;
using UnityEngine;

public abstract class ReviewExercise : TriggerAction
{
    public Section section;

    [SerializeField] private List<TriggerAction> actionsOnCorrect;
    [SerializeField] private List<TriggerAction> actionsOnIncorrect;

    protected override void ExecuteTrigger()
    {
        ReviewAttachment();
    }

    protected abstract void ReviewAttachment();

    protected void TriggerCorrectAnswer(AttachableContainer[] attachableContainers)
    {
        actionsOnCorrect.ForEach(ta => ta?.OnTrigger());

        actionsOnCorrect.Clear();
        actionsOnIncorrect.Clear();

        foreach (var ac in attachableContainers)
        {
            foreach (var ao in ac.attachedObjectInsideCollider)
            {
                ao.gameObject.layer = 0;

                ao.onCorrect.ForEach(ta => ta.OnTrigger());
            }
        }
    }

    protected void TriggerIncorrectAnswer(AttachableContainer[] attachableContainers)
    {
        actionsOnIncorrect.ForEach(ta => ta?.OnTrigger());

        foreach (var ac in attachableContainers)
        {
            foreach (var ao in ac.attachedObjectInsideCollider)
            {
                ao.onIncorrect.ForEach(ta => ta.OnTrigger());
            }
        }
    }
}