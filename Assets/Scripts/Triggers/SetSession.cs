using UnityEngine;

public class SetSession : TriggerAction
{
    [SerializeField] private GameObject[] exercises;

    protected override void ExecuteTrigger()
    {
        var session = GameManager.instance.activeSession;
        if (!session)
        {
            Debug.Log("No active session is declared!");
            return;
        }

        var sessionIndex = session.sessionIndex;
        exercises[sessionIndex - 1].SetActive(true);
    }
}