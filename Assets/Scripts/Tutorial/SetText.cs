using TMPro;
using UnityEngine;

public class SetText : TriggerAction
{
    [SerializeField] private string targetText;
    [SerializeField] private float fontSize = 0.8f;
    [SerializeField] private TextMeshProUGUI targetPanel;

    protected override void ExecuteTrigger()
    {
        if (targetText == "" || !targetPanel)
        {
            Debug.LogWarning("Execution failed: SetText");
            return;
        }

        targetPanel.text = targetText;
        targetPanel.fontSize = fontSize;
    }
}