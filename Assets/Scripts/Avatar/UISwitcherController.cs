using UnityEngine;
using UnityEngine.InputSystem;

public class UISwitcherController : MonoBehaviour
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

        menuGameObject.gameObject.transform.position = new Vector3(
            attachMenuTransform.transform.position.x + attachMenuTransformOffset.x,
            attachMenuTransform.transform.position.y + attachMenuTransformOffset.y,
            attachMenuTransform.transform.position.z + attachMenuTransformOffset.z);
    }

    private void OnDisable()
    {
        inputActionReferenceUISwitcher.action.performed -= ActivateMenuUI;
    }

    private void ActivateMenuUI(InputAction.CallbackContext callback)
    {
        _isUIActive = !_isUIActive;
    }
}