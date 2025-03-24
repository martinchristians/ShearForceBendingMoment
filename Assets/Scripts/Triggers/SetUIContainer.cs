using TMPro;
using UnityEngine;

public class SetUIContainer : TriggerAction
{
    [Header("TASK")] [SerializeField] private GameObject taskPrefab;
    [SerializeField] private GameObject taskContainer;

    [Header("MEASUREMENT")] [SerializeField] private GameObject measurementPrefab;
    [SerializeField] private GameObject measurementContainer;

    [Header("HINT")] [SerializeField] private GameObject hintPrefab;
    [SerializeField] private GameObject hintContainer;

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
        var hintData = SectionData.instance.hintData;
        Debug.Log("hintData: " + hintData + ", Count: " + hintData.infoHints.Count);
        hintData.infoHints.ForEach(h =>
        {
            h.wasShown = true;

            var go = Instantiate(hintPrefab, hintContainer.transform);
            go.GetComponent<TextMeshProUGUI>().text = h.infoText;
            go.SetActive(false);
        });
    }
}