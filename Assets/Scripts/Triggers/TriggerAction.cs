using System.Collections;
using UnityEngine;

public abstract class TriggerAction : MonoBehaviour
{
    [SerializeField] private float delayBeforeTriggering;

    public virtual void OnTrigger()
    {
        if (delayBeforeTriggering > 0f)
            StartCoroutine(ExecuteWithDelay());
        else
            ExecuteTrigger();
    }

    private IEnumerator ExecuteWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeTriggering);
        ExecuteTrigger();
    }

    protected abstract void ExecuteTrigger();
}