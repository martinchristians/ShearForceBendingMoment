using UnityEngine;

public class PlayStopAudio : TriggerAction
{
    private TutorialDataManager _manager;
    private Navigation[] _navigations;

    private bool _isPaused;

    private void Awake()
    {
        _manager = TutorialDataManager.instance;
        _navigations = _manager.navigations;
    }

    protected override void ExecuteTrigger()
    {
        if (GetAudio().isPlaying) Pause();
        else if (_isPaused) Unpause();
        else PlayAudio();
    }

    private AudioSource GetAudio()
    {
        return _navigations[_manager.currentActive].section.GetComponent<SetTutorialScene>().audioSource;
    }

    private void Pause()
    {
        GetAudio().Pause();
        _isPaused = true;
    }

    private void Unpause()
    {
        GetAudio().UnPause();
        _isPaused = false;
    }

    public void PlayAudio()
    {
        GetAudio().Play();
        _isPaused = false;
    }

    public void StopAudio()
    {
        GetAudio().Stop();
        _isPaused = false;
    }
}