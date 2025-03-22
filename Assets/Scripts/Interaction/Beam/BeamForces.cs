using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeamForces : MonoBehaviour
{
    public BeamForceCalculation beamForceCalculation = new();
    public UpdateBeamCalculationUI _updateBeamCalculationUI;

    private AttachableContainer _attachableContainer;
    private List<AttachableObject> _attachedObjectInsideCollider = new();
    private BeamForceDiagrams _beamForceDiagrams;

    private void Awake()
    {
        _attachableContainer = GetComponent<AttachableContainer>();
        _beamForceDiagrams = GetComponent<BeamForceDiagrams>();
    }

    public void UpdateBeamForces()
    {
        _attachedObjectInsideCollider = new List<AttachableObject>();
        _attachedObjectInsideCollider.AddRange(_attachableContainer.attachedObjectInsideCollider);

        GetForcesAndDistances();

        Debug.Log("### Update SFD & BMD ###");
        _beamForceDiagrams.UpdateBeamForceDiagrams();
    }

    private void GetForcesAndDistances()
    {
        var line = _attachableContainer.GetAttachmentLine();
        beamForceCalculation.forcesAndDistancesToStart.Clear();

        beamForceCalculation.beamLength = beamForceCalculation.beamLength > 0
            ? beamForceCalculation.beamLength
            : line.direction.magnitude;

        _attachedObjectInsideCollider.ForEach(
            attachable =>
            {
                var force = attachable.attachableObjectTypeForce;

                var nearestPoint = line.NearestPointToPoint(attachable.transform.position);
                var distanceToStart = (nearestPoint - line.start).magnitude;

                beamForceCalculation.ScaleBeamLength = beamForceCalculation.beamLength / line.direction.magnitude;
                distanceToStart *= beamForceCalculation.ScaleBeamLength;

                beamForceCalculation.forcesAndDistancesToStart.Add(new Vector2(force, distanceToStart));
            }
        );

        beamForceCalculation.forcesAndDistancesToStart.Sort(
            (a, b) => { return Mathf.RoundToInt(Mathf.Sign(a.y - b.y)); }
        );

        beamForceCalculation.CalculateBeamForces();

        UpdateBeamForcesUI();
    }

    private void UpdateBeamForcesUI()
    {
        _updateBeamCalculationUI.beamLength.text = beamForceCalculation.beamLength.ToString("F2") + " m";
        _updateBeamCalculationUI.meter.text = beamForceCalculation.beamLength.ToString("F2") + " m";

        _updateBeamCalculationUI.pinnedSupport.text = beamForceCalculation.supportLeft.ToString("F2") + " N";
        _updateBeamCalculationUI.rollerSupport.text = beamForceCalculation.supportRight.ToString("F2") + " N";

        ClearUIPrefab(_updateBeamCalculationUI.textForceContainer.transform);
        ClearUIPrefab(_updateBeamCalculationUI.textDistanceContainer.transform);
        ClearUIPrefab(_updateBeamCalculationUI.textSFContainer.transform);
        ClearUIPrefab(_updateBeamCalculationUI.textBMContainer.transform);

        foreach (var value in beamForceCalculation.forcesAndDistancesToStart)
        {
            InstantiateUI(_updateBeamCalculationUI.prefabForce_SF,
                _updateBeamCalculationUI.textForceContainer.transform, value.x, " N");

            InstantiateUI(_updateBeamCalculationUI.prefabDistance_BM,
                _updateBeamCalculationUI.textDistanceContainer.transform, value.y, " m");
        }

        foreach (var value in beamForceCalculation.shearForces)
        {
            InstantiateUI(_updateBeamCalculationUI.prefabForce_SF,
                _updateBeamCalculationUI.textSFContainer.transform, value, " N");
        }

        foreach (var value in beamForceCalculation.bendingMoments)
        {
            InstantiateUI(_updateBeamCalculationUI.prefabDistance_BM,
                _updateBeamCalculationUI.textBMContainer.transform, value, " N");
        }
    }

    private void ClearUIPrefab(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

    private void InstantiateUI(GameObject prefab, Transform container, float value, string unit)
    {
        var go = Instantiate(prefab, container);
        go.GetComponent<TextMeshProUGUI>().text = value.ToString("F2") + unit;
    }
}