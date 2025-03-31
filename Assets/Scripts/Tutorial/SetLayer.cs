using UnityEngine;

public class SetLayer : TriggerAction
{
    [SerializeField] private int layer;
    [SerializeField] private GameObject[] gameObjects;

    protected override void ExecuteTrigger()
    {
        if (gameObjects.Length == 0) return;

        foreach (var go in gameObjects) go.layer = layer;
    }
}