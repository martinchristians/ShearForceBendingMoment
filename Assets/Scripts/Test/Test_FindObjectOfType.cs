using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Test_FindObjectOfType : MonoBehaviour
{
    public TeleportationArea TeleportationArea;
    public XRInteractionManager InteractionManager;

    private void Awake()
    {
        if (TeleportationArea == null)
        {
            var test = FindObjectOfType<TeleportationArea>();
            TeleportationArea = test;
        }

        if (InteractionManager == null)
        {
            var test2 = FindObjectOfType<XRInteractionManager>();
            InteractionManager = test2;
        }
    }
}