using UnityEngine;

public class SetKeyKeyboard : TriggerAction
{
    [SerializeField] private KeyStyle keyStyle;
    [SerializeField] private string letter;

    protected override void ExecuteTrigger()
    {
        var keyboardManager = KeyboardManager.instance;

        switch (keyStyle)
        {
            case KeyStyle.NONE:
                Debug.Log("NONE key style!");
                break;
            case KeyStyle.LETTER:
                if (letter != null) keyboardManager.OnLetterButtonClick(letter);
                break;
            case KeyStyle.SPACE:
                keyboardManager.OnSpaceButtonClick();
                break;
            case KeyStyle.BACKSPACE:
                keyboardManager.OnBackspaceButtonClick();
                break;
        }
    }
}