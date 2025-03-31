using System.Collections.Generic;
using UnityEngine;

public class SetTutorialScene : MonoBehaviour
{
    private TutorialDataManager _manager;
    private bool _hasAudioEverFinished;

    public AudioSource audioSource;
    [SerializeField] private AttachableContainer[] attachableContainers;

    public List<TriggerAction> onStart;

    private void Awake()
    {
        _manager = TutorialDataManager.instance;
    }

    private void Start()
    {
        onStart.ForEach(ta => ta?.OnTrigger());
    }

    private void OnEnable()
    {
        _hasAudioEverFinished = _manager.navigations[_manager.currentActive].isDone;
        _manager.CheckFirstTime();
        ResetAttachableObjectInsideContainer();
    }

    private void Update()
    {
        if (_hasAudioEverFinished) return;

        if (!audioSource || audioSource.isPlaying) return;

        if (audioSource.time == 0)
        {
            _hasAudioEverFinished = true;
            _manager.navigations[_manager.currentActive].isDone = true;
            _manager.CheckFirstTime();
        }
    }

    private void ResetAttachableObjectInsideContainer()
    {
        foreach (var ac in attachableContainers)
        {
            foreach (var ao in ac.attachedObjectInsideCollider)
            {
                ao.attachableContainer = null;
                ao.attachableContainers.Clear();
                ao.transform.position = new Vector3(0, -5, 0);

                var rb = ao.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
            }

            ac.attachedObjectInsideCollider.Clear();
        }
    }
}