using UnityEngine;

public class OneTimeBeamForceCalculation : TriggerAction
{
    [SerializeField] private AttachableContainer beam;
    private BeamForces _beamForces;
    private BeamForceDiagrams _beamForceDiagrams;

    private void Awake()
    {
        _beamForces = beam.GetComponent<BeamForces>();
        _beamForceDiagrams = beam.GetComponent<BeamForceDiagrams>();
    }

    protected override void ExecuteTrigger()
    {
        foreach (var ao in beam.attachedObjectInsideCollider)
        {
            ao.SetTransformPreviewToBeam();

            ao.attachableContainer = beam;
            ao.SetTransformToBeam();

            ao.previewRenderer.enabled = false;
            ao.GetComponent<Rigidbody>().isKinematic = true;
        }

        _beamForces.UpdateBeamForces();
        _beamForceDiagrams.UpdateBeamForceDiagrams();

        beam.GetComponent<Collider>().enabled = false;
    }
}