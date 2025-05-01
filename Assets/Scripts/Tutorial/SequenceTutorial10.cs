using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTutorial10 : MonoBehaviour
{
    public SetTutorialScene setTutorialScene;
    private AttachableContainer[] _attachableContainerscontainers;

    [SerializeField] private AttachableObjectType correctTypeBoxLeft;
    [SerializeField] private AttachableObjectType correctTypeBoxRight;
    [SerializeField] private AttachableObjectType correctTypeBoxLeftNext;
    [SerializeField] private AttachableObjectType correctTypeBoxRightNext;

    private bool _isTaskOneComplete;
    private bool _isTaskTwoComplete;

    [SerializeField] private List<TriggerAction> onTaskOneComplete;
    [SerializeField] private List<TriggerAction> onTaskTwoComplete;

    private void Awake()
    {
        _attachableContainerscontainers = setTutorialScene.attachableContainers;
    }

    private void OnEnable()
    {
        _isTaskOneComplete = false;
        _isTaskTwoComplete = false;
    }

    private void Update()
    {
        if (_isTaskOneComplete)
        {
            if (!_isTaskTwoComplete)
                _isTaskTwoComplete =
                    CheckBoxOccupancy(correctTypeBoxLeftNext, correctTypeBoxRightNext, onTaskTwoComplete, 0);
        }
        else
            _isTaskOneComplete = CheckBoxOccupancy(correctTypeBoxLeft, correctTypeBoxRight, onTaskOneComplete, 4);
    }

    private bool CheckBoxOccupancy(AttachableObjectType leftType, AttachableObjectType rightType,
        List<TriggerAction> triggers, int waitTime)
    {
        for (int i = 0; i < _attachableContainerscontainers.Length; i++)
        {
            var container = _attachableContainerscontainers[i];
            if (container.attachedObjectInsideCollider.Count == 0) return false;

            var ao = container.attachedObjectInsideCollider[0];
            if (!ao.isAttachableContainerFilled) return false;

            var expectedType = i == 0 ? leftType : rightType;
            if (ao.attachableObjectType != expectedType) return false;
        }

        if (triggers.Count > 0) triggers.ForEach(t => t?.OnTrigger());
        StartCoroutine(ResetAfterDelay(waitTime));
        return true;
    }

    private IEnumerator ResetAfterDelay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        setTutorialScene.ResetAttachableObjectInsideContainer();
    }
}