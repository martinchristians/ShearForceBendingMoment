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
    private Transform _overlay;

    private void Awake()
    {
        graphicRaycasters = FindObjectsOfType<GraphicRaycaster>(true);
    }

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

                        var overlay = FindComponentInChildWithTag(_raycastHit, "Overlay");
                        overlay.GetComponent<Renderer>().enabled = true;
                        _overlay = overlay;

                        int grabbedLayerMask = LayerMask.GetMask("Grabbed");
                        layerMask |= grabbedLayerMask;
                    }
                }
                else
                {
                    _networkedGrabbing.OnSelectExited();
                    _networkedGrabbing = null;

                    _overlay.GetComponent<Renderer>().enabled = false;

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

    private Transform FindComponentInChildWithTag(RaycastHit hit, string tag)
    {
        for (int i = 0; i < hit.transform.childCount; i++)
        {
            if (hit.transform.GetChild(i).CompareTag(tag))
                return hit.transform.GetChild(i);
        }

        return null;
    }
}