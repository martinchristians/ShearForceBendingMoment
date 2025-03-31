using UnityEngine;
using TMPro;

public class ButtonNavigation : TriggerAction
{
    private TutorialDataManager _manager;
    private Navigation[] _navigations;
    private PlayStopAudio _playStopAudio;

    [SerializeField] private bool isIncreasing;
    [SerializeField] private TextMeshProUGUI textNumberCurrent;

    private void Awake()
    {
        _manager = TutorialDataManager.instance;
        _navigations = _manager.navigations;
        _playStopAudio = _manager.playStopAudio;
    }

    protected override void ExecuteTrigger()
    {
        var currentActive = _manager.currentActive;

        for (int i = 0; i < _navigations.Length; i++)
        {
            if (!_navigations[i].section.activeSelf) continue;

            currentActive = i;
            break;
        }

        if (isIncreasing)
        {
            if (currentActive == _navigations.Length - 1) return;

            ResetGuide();

            var nextActive = currentActive + 1;
            _manager.currentActive = nextActive;

            _navigations[currentActive].section.SetActive(false);
            _navigations[nextActive].section.SetActive(true);

            textNumberCurrent.text = nextActive + 1 + " / " + _navigations.Length;

            _manager.CheckFirstTime();
            if (_manager.navigations[nextActive].isDone)
            {
                var setTutorialScene = _manager.navigations[nextActive].section.GetComponent<SetTutorialScene>();
                setTutorialScene.onStart.ForEach(ta => ta?.OnTrigger());
            }
        }
        else
        {
            if (currentActive == 0) return;

            ResetGuide();

            var nextActive = currentActive - 1;
            _manager.currentActive = nextActive;

            _navigations[currentActive].section.SetActive(false);
            _navigations[nextActive].section.SetActive(true);

            textNumberCurrent.text = nextActive + 1 + " / " + _navigations.Length;

            _manager.CheckFirstTime();
            if (_manager.navigations[nextActive].isDone)
            {
                var setTutorialScene = _manager.navigations[nextActive].section.GetComponent<SetTutorialScene>();
                setTutorialScene.onStart.ForEach(ta => ta?.OnTrigger());
            }
        }
    }

    private void ResetGuide()
    {
        _playStopAudio.StopAudio();
        //_manager.SetUIInitialValue();
    }
}