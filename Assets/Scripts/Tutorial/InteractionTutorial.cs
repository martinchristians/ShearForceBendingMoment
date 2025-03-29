using System.Collections.Generic;
using UnityEngine;

public class InteractionTutorial : MonoBehaviour
{
    [SerializeField] private List<AttachableContainer> attachableContainers;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private GameObject loginPanel;

    private bool isComplete;

    private void Update()
    {
        if (CheckBoxOccupancy())
        {
            taskPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
        else
        {
            taskPanel.SetActive(true);
            loginPanel.SetActive(false);
        }
    }

    private bool CheckBoxOccupancy()
    {
        foreach (var ac in attachableContainers)
        {
            if (ac.attachedObjectInsideCollider.Count == 0) return false;
            if (!ac.attachedObjectInsideCollider[0].isAttachableContainerFilled) return false;
        }

        attachableContainers.ForEach(ac => ac.attachedObjectInsideCollider[0].gameObject.layer = 0);

        return true;
    }
}