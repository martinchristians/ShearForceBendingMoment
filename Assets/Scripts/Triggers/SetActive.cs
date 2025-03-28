using UnityEngine;

public class SetActive : TriggerAction
{
    [SerializeField] private GameObject[] gameObjects;

    protected override void ExecuteTrigger()
    {
        if (gameObjects == null)
        {
            Debug.Log("SetActive TriggerAction isn't executed!");
            return;
        }

        foreach (var go in gameObjects) go?.SetActive(true);
    }
}