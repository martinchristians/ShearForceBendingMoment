using UnityEngine;
using UnityEngine.UI;

public class SetImage : TriggerAction
{
    [SerializeField] private Sprite targetSprite;
    [SerializeField] private Image targetPanel;

    protected override void ExecuteTrigger()
    {
        if (!targetSprite || !targetPanel)
        {
            Debug.LogWarning("Execution failed: SetImage");
            return;
        }

        targetPanel.sprite = targetSprite;
    }
}