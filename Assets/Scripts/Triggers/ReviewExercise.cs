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

    protected void TriggerCorrectAnswer()
    {
        actionsOnCorrect.ForEach(ta => ta?.OnTrigger());

        actionsOnCorrect.Clear();
        actionsOnIncorrect.Clear();
    }

    protected void TriggerIncorrectAnswer()
    {
        actionsOnIncorrect.ForEach(ta => ta?.OnTrigger());
    }
}