using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BeamForceCalculation
{
    public float beamLength;
    public float ScaleBeamLength { get; set; }

    public List<Vector2> forcesAndDistancesToStart = new();
    public int NumForces => forcesAndDistancesToStart.Count;

    public float supportLeft;
    public float supportRight;

    public List<float> shearForces;
    public List<float> bendingMoments;

    public UpdateBeamCalculationUI _updateBeamCalculationUI;

    public float Force(int i) => forcesAndDistancesToStart[i - 1].x;

    public float Distance(int i) => forcesAndDistancesToStart[i - 1].y;

    public void CalculateBeamForces()
    {
        _updateBeamCalculationUI.beamLength.text = beamLength.ToString("F2") + " m";
        _updateBeamCalculationUI.meter.text = beamLength.ToString("F2") + " m";

        CalculateSupports();
        CalculateShearForces();
        CalculateBendingMoments();
    }

    private void CalculateSupports()
    {
        supportLeft = CalculateMomentFromTheLeftSide(false);
        supportRight = CalculateMomentFromTheLeftSide(true);

        _updateBeamCalculationUI.pinnedSupport.text = supportLeft.ToString("F2") + " N";
        _updateBeamCalculationUI.rollerSupport.text = supportRight.ToString("F2") + " N";
    }

    private float CalculateMomentFromTheLeftSide(bool fromLeft)
    {
        var support = 0f;

        for (int i = 1; i <= NumForces; i++)
        {
            var distance = fromLeft
                ? Distance(i)
                : beamLength - Distance(i);

            support += distance * Force(i);
        }

        return support * (1f / beamLength);
    }

    private void CalculateShearForces()
    {
        shearForces = new List<float>();

        shearForces.Add(supportLeft);

        for (int i = 1; i <= NumForces; i++)
        {
            var sf = shearForces[i - 1] - Force(i);
            shearForces.Add(sf);
        }

        shearForces.Add(0);
    }

    private void CalculateBendingMoments()
    {
        bendingMoments = new List<float>();

        bendingMoments.Add(0);

        for (int i = 1; i <= NumForces; i++)
        {
            float bm;
            if (i == 1)
                bm = shearForces[i - 1] * (Distance(i) - 0);
            else
                bm = shearForces[i - 1] * (Distance(i) - Distance(i - 1));

            bendingMoments.Add(bendingMoments[i - 1] + bm);
        }

        bendingMoments.Add(0);
    }
}