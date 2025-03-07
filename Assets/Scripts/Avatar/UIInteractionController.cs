using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UIInteractionController : MonoBehaviour
{
    public GameObject uiCanvasGameObject;

    //public GameObject attachTransform;
    //public Vector3 attachTransformOffset;

    public GameObject uiController;
    public GameObject baseController;

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
            
            /*uiCanvasGameObject.gameObject.transform.position = new Vector3(
                attachTransform.transform.position.x + attachTransformOffset.x,
                attachTransform.transform.position.y + attachTransformOffset.y,
                attachTransform.transform.position.z + attachTransformOffset.z);*/

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