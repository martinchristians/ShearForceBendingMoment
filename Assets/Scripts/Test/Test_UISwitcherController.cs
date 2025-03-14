using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UISwitcherController : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReferenceUISwitcher;
    public GameObject menuGameObject;
    public GameObject attachMenuTransform;
    public Vector3 attachMenuTransformOffset;

    private bool _isUIActive;

    private void Start()
    {
        menuGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputActionReferenceUISwitcher.action.performed += ActivateMenuUI;

        menuGameObject.transform.SetPositionAndRotation(
            attachMenuTransform.transform.position + attachMenuTransformOffset,
            attachMenuTransform.transform.rotation
        );
    }

    private void OnDisable()
    {
        inputActionReferenceUISwitcher.action.performed -= ActivateMenuUI;
    }

    private void ActivateMenuUI(InputAction.CallbackContext callback)
    {
        _isUIActive = !_isUIActive;
        menuGameObject.SetActive(_isUIActive);
    }
}