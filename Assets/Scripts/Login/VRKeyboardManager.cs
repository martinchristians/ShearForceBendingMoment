using System;
using UnityEngine;
using VRKeys;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

public class VRKeyboardManager : MonoBehaviour
{
    public Keyboard keyboard;
    public String welcomeText;

    public GameObject attachTransform;
    public Vector3 attachTransformOffset;

    public TMP_InputField playerNameInputField;

    public GameObject leftBaseController;
    public GameObject rightBaseController;

    public GameObject leftMarret;
    public GameObject rightMarret;

    public void EnableVRKeyboard()
    {
        keyboard.Enable();
        keyboard.SetPlaceholderMessage(welcomeText);

        keyboard.OnUpdate.AddListener(HandleUpdate);
        keyboard.OnSubmit.AddListener(HandleSubmit);
        keyboard.OnCancel.AddListener(HandleCancel);

        keyboard.gameObject.transform.position = new Vector3(
            attachTransform.transform.position.x + attachTransformOffset.x,
            attachTransform.transform.position.y + attachTransformOffset.y,
            attachTransform.transform.position.z + attachTransformOffset.z);

        SetActiveMarrets();

        leftBaseController.GetComponent<XRRayInteractor>().enabled = false;
        rightBaseController.GetComponent<XRRayInteractor>().enabled = false;
    }

    void SetActiveMarrets()
    {
        leftMarret.transform.SetParent(leftBaseController.transform);
        leftMarret.transform.localPosition = Vector3.zero;
        leftMarret.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        leftMarret.SetActive(true);

        rightMarret.transform.SetParent(rightBaseController.transform);
        rightMarret.transform.localPosition = Vector3.zero;
        rightMarret.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        rightMarret.SetActive(true);
    }

    public void DisableVRKeyboard()
    {
        keyboard.OnUpdate.RemoveListener(HandleUpdate);
        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);

        keyboard.Disable();

        DetachMarrets();

        leftBaseController.GetComponent<XRRayInteractor>().enabled = true;
        rightBaseController.GetComponent<XRRayInteractor>().enabled = true;
    }

    void DetachMarrets()
    {
        leftMarret.transform.SetParent(null);
        leftMarret.SetActive(false);

        rightMarret.transform.SetParent(null);
        rightMarret.SetActive(false);
    }

    public void HandleUpdate(string text)
    {
        keyboard.HideValidationMessage();
        playerNameInputField.text = text;
        playerNameInputField.caretPosition = playerNameInputField.text.Length;
    }

    public void HandleSubmit(string text)
    {
        DisableVRKeyboard();

        var eventSystem = EventSystem.current;
        if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(null);
    }

    public void HandleCancel()
    {
        DisableVRKeyboard();

        var eventSystem = EventSystem.current;
        if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(null);
    }
}