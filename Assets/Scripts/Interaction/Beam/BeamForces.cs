using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeamForces : MonoBehaviour
{
    private AttachableContainer _attachableContainer;

    private List<AttachableObject> _attachedObjectInsideCollider = new();

    public BeamForceCalculation beamForceCalculation = new();

    private void Awake()
    {
        _attachableContainer = GetComponent<AttachableContainer>();
    }

    public void UpdateBeamForces()
    {
        _attachedObjectInsideCollider = new List<AttachableObject>();
        _attachedObjectInsideCollider.AddRange(_attachableContainer.attachedObjectInsideCollider);

        GetForcesAndDistances();
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

        //UI
        ClearUIPrefab(beamForceCalculation._updateBeamCalculationUI.textForceContainer.transform);
        ClearUIPrefab(beamForceCalculation._updateBeamCalculationUI.textDistanceContainer.transform);
        ClearUIPrefab(beamForceCalculation._updateBeamCalculationUI.textSFContainer.transform);
        ClearUIPrefab(beamForceCalculation._updateBeamCalculationUI.textBMContainer.transform);

        foreach (var value in beamForceCalculation.forcesAndDistancesToStart)
        {
            InstantiateUI(beamForceCalculation._updateBeamCalculationUI.prefabForce_SF,
                beamForceCalculation._updateBeamCalculationUI.textForceContainer.transform, value.x, " N");

            InstantiateUI(beamForceCalculation._updateBeamCalculationUI.prefabDistance_BM,
                beamForceCalculation._updateBeamCalculationUI.textDistanceContainer.transform, value.y, " m");
        }

        foreach (var value in beamForceCalculation.shearForces)
        {
            InstantiateUI(beamForceCalculation._updateBeamCalculationUI.prefabForce_SF,
                beamForceCalculation._updateBeamCalculationUI.textSFContainer.transform, value, " N");
        }

        foreach (var value in beamForceCalculation.bendingMoments)
        {
            InstantiateUI(beamForceCalculation._updateBeamCalculationUI.prefabDistance_BM,
                beamForceCalculation._updateBeamCalculationUI.textBMContainer.transform, value, " N");
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