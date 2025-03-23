using System.Collections;
using UnityEngine;

public class OneTimeBeamForceCalculation : TriggerAction
{
    [SerializeField] private AttachableContainer beam;
    [SerializeField] private bool keepColliderOn;
    private BeamForces _beamForces;

    private void Awake()
    {
        _beamForces = beam.GetComponent<BeamForces>();
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

        _beamForces.UpdateBeamForces(true);

        if (!keepColliderOn)
            beam.GetComponent<Collider>().enabled = false;
        else
            StartCoroutine(ClearBeamAfterWeightDisabled());
    }

    private IEnumerator ClearBeamAfterWeightDisabled()
    {
        yield return new WaitForSeconds(.3f);
        beam.attachedObjectInsideCollider.Clear();
    }
}