using UnityEngine;

public class SetDeactive : TriggerAction
{
    [SerializeField] private GameObject[] gameObjects;

    protected override void ExecuteTrigger()
    {
        if (gameObjects == null)
        {
            Debug.Log("SetDeactive TriggerAction isn't executed!");
            return;
        }

        foreach (var go in gameObjects) go.SetActive(false);
    }
}