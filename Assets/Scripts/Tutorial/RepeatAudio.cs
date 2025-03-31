using UnityEngine;

public class RepeatAudio : TriggerAction
{
    private TutorialDataManager _manager;
    private Navigation[] _navigations;

    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        _manager = TutorialDataManager.instance;
        _navigations = _manager.navigations;
    }

    protected override void ExecuteTrigger()
    {
        var currentActive = _manager.currentActive;

        audioSource = _navigations[currentActive].section.GetComponent<SetTutorialScene>().audioSource;
        audioSource.Stop();
        audioSource.time = 0;
        audioSource.Play();
    }
}