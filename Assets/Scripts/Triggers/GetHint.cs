using TMPro;
using UnityEngine;

public class GetHint : TriggerAction
{
    [SerializeField] private GameObject hintContainer;

    protected override void ExecuteTrigger()
    {
        var hintsData = SectionDataManager.instance.hintDataList;
        var numHintsData = hintsData.Count;

        //Deactivate all task-gameobjects
        foreach (Transform child in hintContainer.transform)
            child.gameObject.SetActive(false);

        var nextHintDataToShow = numHintsData - 1;
        for (int i = 0; i < numHintsData; i++)
        {
            var hintData = hintsData[i];

            if (hintData.wasShown == false)
            {
                hintData.wasShown = true;
                nextHintDataToShow = i;
                break;
            }
        }

        var hintGameObject = hintContainer.transform.GetChild(nextHintDataToShow).gameObject;
        hintGameObject.SetActive(true);

        hintGameObject.GetComponent<TextMeshProUGUI>().text = hintsData[nextHintDataToShow].infoText;

        var triggerActionList = hintsData[nextHintDataToShow].triggerList;
        triggerActionList.ForEach(ta => ta?.OnTrigger());
    }
}