using System.Collections.Generic;
using UnityEngine;

public class TriggerOnStart : MonoBehaviour
{
    public List<TriggerAction> triggerOnStartList;

    private void Start()
    {
        triggerOnStartList.ForEach(t => t?.OnTrigger());
    }
}