using System.Collections.Generic;
using TMPro;
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
        var session = GameManager.instance.session;

        InstantiateTaskContainer(session);
        InstantiateMeasurementContainer(session);
        InstantiateHintContainer();
    }

    private void InstantiateTaskContainer(Session session)
    {
        session.sections.ForEach(s =>
        {
            var go = Instantiate(taskPrefab, taskContainer.transform);
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = s.title;
            go.transform.GetChild(1).gameObject.SetActive(true);
            go.transform.GetChild(2).gameObject.SetActive(false);
        });
    }

    private void InstantiateMeasurementContainer(Session session)
    {
        session.sections.ForEach(s =>
        {
            var go = Instantiate(measurementPrefab, measurementContainer.transform);
            go.GetComponent<MeasurementData>().title.text = s.title;
        });
    }

    private void InstantiateHintContainer()
    {
        var infoHintData = SectionData.instance.infoHintData;
        SectionData.instance.hintDataList = new();

        for (int i = 0; i < infoHintData.infoTextList.Count; i++)
        {
            var go = Instantiate(hintPrefab, hintContainer.transform);
            go.GetComponent<TextMeshProUGUI>().text = infoHintData.infoTextList[i];
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