using System.Collections.Generic;
using UnityEngine;

public class SetUIContainer : TriggerAction
{
    [System.Serializable]
    public struct HintTrigger
    {
        public int hintIndex;
        public List<TriggerAction> hintTriggerActionList;
    }

    [Header("TASK")] [SerializeField] private GameObject taskPrefab;
    [SerializeField] private GameObject taskContainer;

    [Header("MEASUREMENT")] [SerializeField] private GameObject measurementPrefab;
    [SerializeField] private GameObject measurementContainer;

    [Header("HINT")] [SerializeField] private GameObject hintPrefab;
    [SerializeField] private GameObject hintContainer;
    [SerializeField] private List<HintTrigger> hintTriggers = new();

    protected override void ExecuteTrigger()
    {
        var session = GameManager.instance.activeSession;

        InstantiateTaskContainer(session);
        InstantiateMeasurementContainer(session);
        InstantiateHintContainer();
    }

    private void InstantiateTaskContainer(Session session)
    {
        var activeSectionIndex = SectionData.instance.section.sectionIndex;
        for (int i = 0; i < session.sections.Count; i++)
        {
            var section = session.sections[i];

            var go = Instantiate(taskPrefab, taskContainer.transform);
            var taskData = go.GetComponent<TaskData>();
            taskData.title.text = section.title;
            taskData.undoneTask.gameObject.SetActive(true);
            taskData.doneTask.gameObject.SetActive(false);

            if (i == activeSectionIndex - 1) SectionData.instance.taskData = taskData;
        }
    }

    private void InstantiateMeasurementContainer(Session session)
    {
        var activeSectionIndex = SectionData.instance.section.sectionIndex;
        for (int i = 0; i < session.sections.Count; i++)
        {
            var section = session.sections[i];

            var go = Instantiate(measurementPrefab, measurementContainer.transform);
            var measurementData = go.GetComponent<MeasurementData>();
            measurementData.title.text = section.title;

            if (i == activeSectionIndex - 1) SectionData.instance.measurementData = measurementData;
        }
    }

    private void InstantiateHintContainer()
    {
        var infoHintData = SectionData.instance.infoHintData;
        SectionData.instance.hintDataList = new();

        for (int i = 0; i < infoHintData.infoTextList.Count; i++)
        {
            var go = Instantiate(hintPrefab, hintContainer.transform);
            go.SetActive(false);

            //Check if any triggerAction should be executed when the hint is active
            List<TriggerAction> tempTriggerActionList = new();
            if (hintTriggers.Count != 0)
            {
                hintTriggers.ForEach(ht =>
                {
                    if (ht.hintIndex == i)
                    {
                        foreach (var ta in ht.hintTriggerActionList) tempTriggerActionList.Add(ta);
                    }
                });
            }

            //Create List<Hint> for GetHint() to retrieve
            HintData newHintData = new HintData
            {
                infoText = infoHintData.infoTextList[i],
                wasShown = false,
                triggerList = tempTriggerActionList
            };

            SectionData.instance.hintDataList.Add(newHintData);
        }
    }
}