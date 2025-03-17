using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableContainer : MonoBehaviourPunCallbacks
{
    public Renderer emptyBox;
    public Renderer filledBox;
    public Transform attachTransform;
    public bool isDisplayPreview;

    public int containerIndex = -1;
    public bool allowMoreThanOneAttachable;
    public List<AttachableObject> attachedObjectInsideCollider = new();

    private AttachableObject _attachable;

    private void Update()
    {
        if (attachedObjectInsideCollider.Count > 0)
        {
            if (isDisplayPreview)
            {
                _attachable.SetTransformPreviewToAttachableContainer();
                return;
            }

            emptyBox.enabled = false;
            filledBox.enabled = true;
        }
        else
        {
            if (isDisplayPreview)
            {
                return;
            }

            emptyBox.enabled = true;
            filledBox.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var attachable = other.gameObject.GetComponent<AttachableObject>();
        if (!attachable) return;

        if (attachedObjectInsideCollider.Count > 0 && !allowMoreThanOneAttachable) return;

        attachedObjectInsideCollider.Add(attachable);
        attachable.attachableContainers.Add(this);

        if (isDisplayPreview)
        {
            attachable.previewRenderer.enabled = true;
            _attachable = attachable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var attachable = other.gameObject.GetComponent<AttachableObject>();
        if (!attachable) return;

        attachedObjectInsideCollider.Remove(attachable);
        attachable.attachableContainers.Remove(this);

        if (isDisplayPreview)
        {
            attachable.previewRenderer.enabled = false;
        }
    }
}