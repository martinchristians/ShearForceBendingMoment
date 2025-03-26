using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachableContainer : MonoBehaviourPunCallbacks
{
    public List<AttachableObject> attachedObjectInsideCollider = new();
    public Transform attachTransform;
    public bool isDisplayOnBeam;
    public bool isDisplayPreview;

    public AttachableObject attachable
    {
        get => _attachable;
        set => _attachable = value;
    }
    private AttachableObject _attachable;

    [Header("Attachable Box")] public int containerIndex = -1;
    public Renderer emptyBox;
    public Renderer filledBox;

    [Header("Attachable Beam")] public bool isUpdatingDiagrams = true;
    public Transform startPoint;
    public Transform endPoint;

    [Header("TriggerAction")] public List<TriggerAction> attachedTriggerActions;
    public List<TriggerAction> deattachedTriggerActions;

    private void Awake()
    {
        if (isDisplayOnBeam) isDisplayPreview = true;
    }

    private void Update()
    {
        if (!isDisplayOnBeam)
        {
            //Attachable box

            if (!isDisplayPreview)
            {
                if (attachedObjectInsideCollider.Count == 1)
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
            else
            {
                if (!_attachable) return;

                _attachable.SetTransformPreviewToAttachableContainer();
            }
        }
        else
        {
            //Attachable beam

            if (!_attachable) return;

            _attachable.SetTransformPreviewToBeam();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var ao = other.gameObject.GetComponent<AttachableObject>();
        if (!ao) return;

        if (!isDisplayOnBeam)
        {
            //Attachable box

            if (attachedObjectInsideCollider.Count == 1) return;

            if (isDisplayPreview)
                ao.previewRenderer.enabled = true;
        }
        else
        {
            //Attachable beam   

            foreach (var _ in attachedObjectInsideCollider)
            {
                if (attachedObjectInsideCollider.Contains(ao)) return;
            }

            ao.previewRenderer.enabled = true;
        }

        _attachable = ao;

        attachedObjectInsideCollider.Add(ao);
        ao.attachableContainers.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var attachable = other.gameObject.GetComponent<AttachableObject>();
        if (!attachable) return;

        if (!isDisplayOnBeam)
        {
            //Attachable box

            if (isDisplayPreview)
                attachable.previewRenderer.enabled = false;
        }
        else
        {
            //Attachable beam  

            attachable.previewRenderer.enabled = false;
        }

        _attachable = null;

        attachedObjectInsideCollider.Remove(attachable);
        attachable.attachableContainers.Remove(this);
    }

    public BeamLine GetAttachmentLine()
    {
        var line = new BeamLine();
        line.Set(startPoint.position, endPoint.position);
        return line;
    }
}