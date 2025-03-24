using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HintData", menuName = "Exercise/Hint")]
public class HintData : ScriptableObject
{
    [System.Serializable]
    public struct HintInfo
    {
        public string infoText;
        public bool wasShown;
    }

    public List<HintInfo> infoHints;
}