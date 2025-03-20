using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableObject : MonoBehaviourPunCallbacks
{
    public AttachableObjectType attachableObjectType;
    private float _attachableObjectTypeForce;
    public float attachableObjectTypeForce => _attachableObjectTypeForce;

    public AttachableContainer attachableContainer;
    public bool isAttachableContainerFilled => attachableContainer != null;
    public List<AttachableContainer> attachableContainers = new();
    public bool isAttachableContainersFilled => attachableContainers.Count > 0;

    public Renderer previewRenderer;
    public Transform transformAttachBoxContainerOffset;
    public Transform transformAttachBoxContainerPreviewOffset;
    public Transform transformAttachBeamContainerPreviewOffset;

    private Transform _transform;
    private Transform _transformPreview;

    private BeamForces _beamForces;
    private BeamForceDiagrams _beamForceDiagrams;

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
            if (attachableContainer.isCalculatingForces)
            {
                _beamForces = attachableContainer.GetComponent<BeamForces>();
                _beamForceDiagrams = attachableContainer.GetComponent<BeamForceDiagrams>();

                if (!_beamForces && !_beamForceDiagrams) return;

                Debug.Log("### Update beam forces ###");
                _beamForces.UpdateBeamForces();
                Debug.Log("### Update SFD & BMD ###");
                _beamForceDiagrams.UpdateBeamForceDiagrams();
            }
        }
    }

    public void Detach()
    {
        if (!attachableContainer.isDisplayOnBeam)
        {
            //Attachable box
        }
        else
        {
            //Attachable beam

            if (attachableContainer.isCalculatingForces)
            {
                Debug.Log("### Update beam forces ###");
                _beamForces.UpdateBeamForces();
                Debug.Log("### Update SFD & BMD ###");
                _beamForceDiagrams.UpdateBeamForceDiagrams();

                _beamForces = null;
                _beamForceDiagrams = null;
            }
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