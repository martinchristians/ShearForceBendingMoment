using UnityEngine;
using TMPro;

[System.Serializable]
public class UpdateBeamCalculationUI
{
    public TextMeshProUGUI beamLength;
    public TextMeshProUGUI meter;
    public TextMeshProUGUI pinnedSupport;
    public TextMeshProUGUI rollerSupport;

    public Transform textForceContainer;
    public Transform textDistanceContainer;
    public Transform textSFContainer;
    public Transform textBMContainer;

    public GameObject prefabForce_SF;
    public GameObject prefabDistance_BM;
}