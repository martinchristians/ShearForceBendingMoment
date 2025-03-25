using UnityEngine;

public class SetTimer : TriggerAction
{
    [SerializeField] private bool isStartTimer;

    protected override void ExecuteTrigger()
    {
        SectionData.instance.isStartTimer = isStartTimer;
    }
}