using StarterAssets;
using UnityEngine;

public class SetControllerActive : TriggerAction
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private FirstPersonController firstPersonController;

    protected override void ExecuteTrigger()
    {
        if (isActive)
            firstPersonController.enabled = true;
        else
            firstPersonController.enabled = false;
    }
}