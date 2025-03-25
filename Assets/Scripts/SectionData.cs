using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintData
{
    public string infoText;
    public bool wasShown;
    public List<TriggerAction> triggerList;
}

public class SectionData : MonoBehaviour
{
    public Section section;
    public InfoHintData infoHintData;
    public List<HintData> hintDataList;

    public static SectionData instance;

    private void Awake()
    {
        hintDataList = null;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}