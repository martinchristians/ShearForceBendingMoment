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

    [Header("MEASUREMENT")] [SerializeField]
    private GameObject measurementPrefab;

    [SerializeField] private GameObject measurementContainer;

    [Header("HINT")] [SerializeField] private GameObject hintPrefab;
    [SerializeField] private GameObject hintContainer;
    [SerializeField] private List<HintTrigger> hintTriggers = new();

    protected override void ExecuteTrigger()
    {
        var session = SessionDataManager.instance.activeSession;

        var taskPrefabs = InstantiateTaskContainer(session);
        var measurementPrefabs = InstantiateMeasurementContainer(session);
        InstantiateHintContainer();
        RestoreSectionData(taskPrefabs, measurementPrefabs);
    }

    private List<GameObject> InstantiateTaskContainer(Session session)
    {
        List<GameObject> prefabs = new();

        var activeSectionIndex = SessionDataManager.instance.activeSection.sectionIndex;
        for (int i = 0; i < session.sections.Count; i++)
        {
            var section = session.sections[i];

            var go = Instantiate(taskPrefab, taskContainer.transform);
            var taskData = go.GetComponent<TaskData>();
            taskData.title.text = section.title;
            taskData.undoneTask.gameObject.SetActive(true);
            taskData.doneTask.gameObject.SetActive(false);

            prefabs.Add(go);

            if (i == activeSectionIndex - 1) SectionDataManager.instance.taskData = taskData;
        }

        return prefabs;
    }

    private List<GameObject> InstantiateMeasurementContainer(Session session)
    {
        List<GameObject> prefabs = new();

        var activeSectionIndex = SessionDataManager.instance.activeSection.sectionIndex;
        for (int i = 0; i < session.sections.Count; i++)
        {
            var section = session.sections[i];

            var go = Instantiate(measurementPrefab, measurementContainer.transform);
            var measurementData = go.GetComponent<MeasurementData>();
            measurementData.title.text = section.title;

            prefabs.Add(go);

            if (i == activeSectionIndex - 1) SectionDataManager.instance.measurementData = measurementData;
        }

        return prefabs;
    }

    private void InstantiateHintContainer()
    {
        var infoHintData = SessionDataManager.instance.activeHintData;
        SectionDataManager.instance.hintDataList = new();

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

            SectionDataManager.instance.hintDataList.Add(newHintData);
        }
    }

    private void RestoreSectionData(List<GameObject> prefabTask, List<GameObject> prefabMeasurement)
    {
        SectionDataManager.instance.storedSectionDatas.ForEach(data =>
        {
            if (!data.isRestoreValue) return;

            //Restore section data when player still on the same session
            if (data.session != SessionDataManager.instance.activeSession) return;

            var sessionList = SessionDataManager.instance.activeSession.sections;
            for (int i = 0; i < sessionList.Count; i++)
            {
                var section = sessionList[i];
                if (data.section.Equals(section))
                {
                    var TaskData = prefabTask[i].GetComponent<TaskData>();
                    if (data.isTaskDone)
                    {
                        TaskData.doneTask.enabled = true;
                        TaskData.undoneTask.enabled = false;
                    }

                    var measurementData = prefabMeasurement[i].GetComponent<MeasurementData>();
                    measurementData.timer.text = string.Format("{0:0}:{1:00}", data.minutes, data.seconds);
                    measurementData.attempt.text = data.attempt.ToString();
                    measurementData.mistake.text = data.mistake.ToString();
                    measurementData.score.text = data.score.ToString();
                }
            }
        });
    }
}