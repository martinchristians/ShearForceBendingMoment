using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableObject : MonoBehaviourPunCallbacks
{
    public AttachableContainer attachableContainer;
    public bool isAttachableContainerFilled => attachableContainer != null;

    public List<AttachableContainer> attachableContainers = new();
    public bool isAttachableContainersFilled => attachableContainers.Count > 0;

    public Renderer previewRenderer;
    public Transform transformAttachContainerOffset;
    public Transform transformAttachPreviewContainerOffset;

    private Transform _transform;
    private Transform _transformPreview;

    private void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
        _transformPreview = previewRenderer.GetComponent<Transform>();
    }

    public void Attach()
    {
        attachableContainer = attachableContainers[0];

        SetTransformToAttachableContainer();

        if (attachableContainer.isDisplayPreview)
        {
            previewRenderer.enabled = false;
        }
    }

    public void Detach()
    {
        attachableContainer = null;
    }

    public void SetTransformToAttachableContainer()
    {
        if (!attachableContainer.isDisplayPreview)
        {
            _transform.position = attachableContainer.attachTransform.position;
            _transform.rotation = attachableContainer.attachTransform.rotation;

            _transform.position += transformAttachContainerOffset.localPosition;
            _transform.rotation *= transformAttachContainerOffset.localRotation;
        }
        else
        {
            _transform.position = attachableContainer.attachTransform.position;
            _transform.rotation = attachableContainer.attachTransform.rotation;

            _transform.position += transformAttachPreviewContainerOffset.localPosition;
            _transform.rotation *= transformAttachPreviewContainerOffset.localRotation;
        }
    }

    public void SetTransformPreviewToAttachableContainer()
    {
        _transformPreview.position = attachableContainers[0].attachTransform.position;
        _transformPreview.rotation = attachableContainers[0].attachTransform.rotation;

        _transformPreview.position += transformAttachPreviewContainerOffset.localPosition;
        _transformPreview.rotation *= transformAttachPreviewContainerOffset.localRotation;
    }
}