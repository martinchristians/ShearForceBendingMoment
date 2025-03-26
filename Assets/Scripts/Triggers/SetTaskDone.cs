public class SetTaskDone : TriggerAction
{
    protected override void ExecuteTrigger()
    {
        SectionDataManager.instance.isTaskDone = true;

        var taskData = SectionDataManager.instance.taskData;
        taskData.doneTask.gameObject.SetActive(true);
        taskData.undoneTask.gameObject.SetActive(false);
    }
}