using System;
using TMPro;
using UnityEngine;

public enum KeyStyle
{
    NONE,
    LETTER,
    SPACE,
    BACKSPACE
}

public class KeyboardManager : MonoBehaviour
{
    public String username = "";

    [SerializeField] private TMP_InputField inputField;

    public static KeyboardManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnLetterButtonClick(string letter)
    {
        if (!inputField)
        {
            Debug.LogWarning("Input field reference is missing in Manager!");
            return;
        }

        inputField.text += letter;
        username = inputField.text;
    }

    public void OnSpaceButtonClick()
    {
        if (!inputField)
        {
            Debug.LogWarning("Input field reference is missing in Manager!");
            return;
        }

        inputField.text += " ";
        username = inputField.text;
    }

    public void OnBackspaceButtonClick()
    {
        if (!inputField)
        {
            Debug.LogWarning("Input field reference is missing in Manager!");
            return;
        }

        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        username = inputField.text;
    }
}