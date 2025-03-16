using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableContainer : MonoBehaviourPunCallbacks
{
    public Renderer emptyBox;
    public Renderer filledBox;
    public Transform attachTransform;

    public bool allowMoreThanOneAttachable;
    public List<AttachableObject> attachedObjectInsideCollider = new();

    private void Update()
    {
        if (attachedObjectInsideCollider.Count > 0)
        {
            emptyBox.enabled = false;
            filledBox.enabled = true;
        }
        else
        {
            emptyBox.enabled = true;
            filledBox.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var attachable = other.gameObject.GetComponent<AttachableObject>();
        if (!attachable) return;

        if (attachedObjectInsideCollider.Count > 0 && !allowMoreThanOneAttachable) return;

        attachedObjectInsideCollider.Add(attachable);
        attachable.attachableContainers.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var attachable = other.gameObject.GetComponent<AttachableObject>();
        if (!attachable) return;

        attachedObjectInsideCollider.Remove(attachable);
        attachable.attachableContainers.Remove(this);
    }
}