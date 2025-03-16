using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableObject : MonoBehaviourPunCallbacks
{
    public AttachableContainer attachableContainer;
    public bool isAttachableContainerFilled => attachableContainer != null;

    public List<AttachableContainer> attachableContainers = new();
    public bool isAttachableContainersFilled => attachableContainers.Count > 0;

    private Transform _transform;
    public Transform transformAttachContainerOffset;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    public void Attach()
    {
        attachableContainer = attachableContainers[0];

        SetTransformToAttachableContainer();
    }

    public void Detach()
    {
        attachableContainer = null;
    }

    public void SetTransformToAttachableContainer()
    {
        _transform.position = attachableContainer.attachTransform.position;
        _transform.rotation = attachableContainer.attachTransform.rotation;

        _transform.position += transformAttachContainerOffset.localPosition;
        _transform.rotation *= transformAttachContainerOffset.localRotation;
    }
}