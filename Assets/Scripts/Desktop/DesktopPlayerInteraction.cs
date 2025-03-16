using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DesktopPlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GraphicRaycaster[] graphicRaycasters;

    const float MaxRayCastDistance = 30f;

    private NetworkedGrabbing _networkedGrabbing;
    private RaycastHit _raycastHit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //UI layer
            if (IsPointingAtUI()) return;

            //Interactable layer
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out _raycastHit,
                    MaxRayCastDistance, layerMask))
            {
                if (!_networkedGrabbing)
                {
                    if (_raycastHit.transform.TryGetComponent(out _networkedGrabbing))
                    {
                        _networkedGrabbing.OnSelectEntered(mainCamera);

                        int grabbedLayerMask = LayerMask.GetMask("Grabbed");
                        layerMask |= grabbedLayerMask;
                    }
                }
                else
                {
                    _networkedGrabbing.OnSelectExited();
                    _networkedGrabbing = null;

                    int grabbedLayerMask = LayerMask.GetMask("Grabbed");
                    layerMask &= ~grabbedLayerMask;
                }
            }
        }
    }

    private bool IsPointingAtUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Screen.width / 2, Screen.height / 2)
        };

        List<RaycastResult> results = new List<RaycastResult>();

        foreach (GraphicRaycaster gr in graphicRaycasters)
        {
            if (!gr) continue;

            gr.Raycast(pointerData, results);
        }

        foreach (var result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button)
            {
                button.onClick.Invoke();
                return true;
            }
        }

        return false;
    }
}