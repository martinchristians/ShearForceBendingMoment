using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableObject : MonoBehaviourPunCallbacks
{
    public AttachableObjectType attachableObjectType;

    public float attachableObjectTypeForce => _attachableObjectTypeForce;
    private float _attachableObjectTypeForce;

    public bool isAttachableContainerFilled => attachableContainer != null;
    public AttachableContainer attachableContainer;

    public bool isAttachableContainersFilled => attachableContainers.Count > 0;
    public List<AttachableContainer> attachableContainers = new();

    public Renderer previewRenderer;
    public Transform transformAttachBoxContainerOffset;
    public Transform transformAttachBoxContainerPreviewOffset;
    public Transform transformAttachBeamContainerPreviewOffset;

    private Transform _transform;
    private Transform _transformPreview;

    private BeamForces _beamForces;

    [Header("TriggerAction")] public List<TriggerAction> onAttached;
    public List<TriggerAction> onDetached;
    public List<TriggerAction> onCorrect;
    public List<TriggerAction> onIncorrect;

    private void Awake()
    {
        attachableContainer = null;
        attachableContainers.Clear();

        if (attachableObjectType == AttachableObjectType.SPOTLIGHT)
            _attachableObjectTypeForce = AttachableObjectConstants.SPOTLIGHT_FORCE;
        else if (attachableObjectType == AttachableObjectType.MOVING_HEAD)
            _attachableObjectTypeForce = AttachableObjectConstants.MOVING_HEAD_FORCE;
        else if (attachableObjectType == AttachableObjectType.SPEAKER)
            _attachableObjectTypeForce = AttachableObjectConstants.SPEAKER_FORCE;

        _transform = gameObject.GetComponent<Transform>();
        _transformPreview = previewRenderer.GetComponent<Transform>();
    }

    public void Attach()
    {
        attachableContainer = attachableContainers[0];
        attachableContainer.attachable = null;

        //Execute triggerAction
        attachableContainer.attachedTriggerActions.ForEach(ta => ta?.OnTrigger());
        onAttached.ForEach(ta => ta?.OnTrigger());

        if (!attachableContainer.isDisplayOnBeam)
        {
            //Attachable box

            if (attachableContainer.isDisplayPreview)
            {
                previewRenderer.enabled = false;
            }

            SetTransformToAttachableContainer();
        }
        else
        {
            //Attachable beam

            previewRenderer.enabled = false;

            SetTransformToBeam();

            //Start forces calculation
            _beamForces = attachableContainer.GetComponent<BeamForces>();

            if (!_beamForces) return;

            Debug.Log("### Update beam forces ###");
            _beamForces.UpdateBeamForces(attachableContainer.isUpdatingDiagrams);
        }
    }

    public void Detach()
    {
        if (attachableContainer == null) return;

        //Execute triggerAction
        attachableContainer.deattachedTriggerActions.ForEach(ta => ta?.OnTrigger());
        onDetached.ForEach(ta => ta?.OnTrigger());

        if (!attachableContainer.isDisplayOnBeam)
        {
            //Attachable box
        }
        else
        {
            //Attachable beam

            Debug.Log("### Update beam forces ###");
            _beamForces.UpdateBeamForces(attachableContainer.isUpdatingDiagrams);
        }

        attachableContainer = null;
    }

    private void SetTransformToAttachableContainer()
    {
        if (!attachableContainer.isDisplayPreview)
        {
            _transform.position = attachableContainer.attachTransform.position;
            _transform.rotation = attachableContainer.attachTransform.rotation;

            _transform.position += transformAttachBoxContainerOffset.localPosition;
            _transform.rotation *= transformAttachBoxContainerOffset.localRotation;
        }
        else
        {
            _transform.position = attachableContainer.attachTransform.position;
            _transform.rotation = attachableContainer.attachTransform.rotation;

            _transform.position += transformAttachBoxContainerPreviewOffset.localPosition;
            _transform.rotation *= transformAttachBoxContainerPreviewOffset.localRotation;
        }
    }

    public void SetTransformPreviewToAttachableContainer()
    {
        _transformPreview.position = attachableContainers[0].attachTransform.position;
        _transformPreview.rotation = attachableContainers[0].attachTransform.rotation;

        _transformPreview.position += transformAttachBoxContainerPreviewOffset.localPosition;
        _transformPreview.rotation *= transformAttachBoxContainerPreviewOffset.localRotation;
    }

    public void SetTransformPreviewToBeam()
    {
        var line = attachableContainers[0].GetAttachmentLine();
        var nearestPoint = line.NearestPointToPoint(_transformPreview.position);
        _transformPreview.position = nearestPoint + transformAttachBeamContainerPreviewOffset.localPosition;

        attachableContainers[0].attachTransform.position = _transformPreview.position;
    }

    public void SetTransformToBeam()
    {
        _transform.position = attachableContainer.attachTransform.position;
        _transform.rotation = transformAttachBeamContainerPreviewOffset.localRotation;
    }

    public void SetAttachableObjectUpdateableInsideBeamCollider()
    {
        if (!attachableContainer) return;
        if (!attachableContainer.isDisplayOnBeam) return;

        attachableContainer.attachable = this;
        previewRenderer.enabled = true;
    }
}