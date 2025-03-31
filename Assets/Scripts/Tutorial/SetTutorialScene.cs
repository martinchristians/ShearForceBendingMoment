using System.Collections.Generic;
using UnityEngine;

public class SetTutorialScene : MonoBehaviour
{
    private TutorialDataManager _manager;
    [SerializeField] private bool _hasAudioEverFinished;

    public AudioSource audioSource;
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
}