using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UIInteractionController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvasGameObject;

    [SerializeField] private GameObject uiController;
    [SerializeField] private GameObject baseController;

    private bool _isUICanvasActive;
    [SerializeField] private InputActionReference inputActionReferenceUISwitcher;

    private void OnEnable()
    {
        inputActionReferenceUISwitcher.action.performed += ActivateteUIMode;
    }

    private void OnDisable()
    {
        inputActionReferenceUISwitcher.action.performed -= ActivateteUIMode;
    }

    private void Start()
    {
        //Deactivate UI Canvas by default
        if (uiCanvasGameObject != null) uiCanvasGameObject.SetActive(false);

        //Deactivate UI Controller by default
        uiController.GetComponent<XRRayInteractor>().enabled = false;
        uiController.GetComponent<XRInteractorLineVisual>().enabled = false;
    }

    private void ActivateteUIMode(InputAction.CallbackContext o)
    {
        if (!_isUICanvasActive)
        {
            _isUICanvasActive = true;
            uiCanvasGameObject.SetActive(true);

            uiController.GetComponent<XRRayInteractor>().enabled = true;
            uiController.GetComponent<XRInteractorLineVisual>().enabled = true;
            baseController.GetComponent<XRRayInteractor>().enabled = false;
        }
        else
        {
            _isUICanvasActive = false;
            uiCanvasGameObject.SetActive(false);

            uiController.GetComponent<XRRayInteractor>().enabled = false;
            uiController.GetComponent<XRInteractorLineVisual>().enabled = false;
            baseController.GetComponent<XRRayInteractor>().enabled = true;
        }
    }
}