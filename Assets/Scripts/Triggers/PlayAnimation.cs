using UnityEngine;

public class PlayAnimation : TriggerAction
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animTrigger;

    protected override void ExecuteTrigger()
    {
        if (!animator || string.IsNullOrEmpty(animTrigger))
        {
            Debug.Log("PlayAnimation TriggerAction isn't executed!");
            return;
        }

        animator.Play(animTrigger);
    }
}