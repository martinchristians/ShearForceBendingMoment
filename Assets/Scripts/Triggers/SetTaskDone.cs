public class SetTaskDone : TriggerAction
{
    protected override void ExecuteTrigger()
    {
        SectionData.instance.isTaskDone = true;

        var taskData = SectionData.instance.taskData;
        taskData.doneTask.gameObject.SetActive(true);
        taskData.undoneTask.gameObject.SetActive(false);
    }
}