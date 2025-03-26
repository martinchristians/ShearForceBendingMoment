public class SetTaskDone : TriggerAction
{
    protected override void ExecuteTrigger()
    {
        SectionDataManager.instance.UpdateTaskState();
    }
}