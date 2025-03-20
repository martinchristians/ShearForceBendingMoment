using System.Collections.Generic;
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
        beamForceCalculation.beamLength = line.direction.magnitude;

        for (int i = 0; i < _attachedObjectInsideCollider.Count; i++)
        {
            var attachable = _attachedObjectInsideCollider[i];

            var force = attachable.attachableObjectTypeForce;

            var nearestPoint = line.NearestPointToPoint(attachable.transform.position);
            var distanceToStart = (nearestPoint - line.start).magnitude;

            beamForceCalculation.forcesAndDistancesToStart.Add(new Vector2(force, distanceToStart));
        }

        beamForceCalculation.forcesAndDistancesToStart.Sort(
            (a, b) => { return Mathf.RoundToInt(Mathf.Sign(a.y - b.y)); }
        );

        beamForceCalculation.CalculateBeamForces();
    }
}