using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintData
{
    public string infoText;
    public bool wasShown;
    public List<TriggerAction> triggerList;
}

public class SectionData : MonoBehaviour
{
    public Section section;

    [HideInInspector] public TaskData taskData;
    [HideInInspector] public bool isTaskDone;

    [HideInInspector] public MeasurementData measurementData;
    [SerializeField] private float startTime = 180f;
    private float _currentTime;
    [HideInInspector] public bool isStartTimer;
    [HideInInspector] public int minutes;
    [HideInInspector] public int seconds;
    [HideInInspector] public int attempt;
    [HideInInspector] public int mistake;
    [HideInInspector] public int score = 100;

    public InfoHintData infoHintData;
    [HideInInspector] public List<HintData> hintDataList;

    public static SectionData instance;

    private void Awake()
    {
        hintDataList = null;
        _currentTime = startTime;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (isStartTimer)
        {
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                isStartTimer = false;
                UpdateTimerDisplay();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        minutes = Mathf.FloorToInt(_currentTime / 60);
        seconds = Mathf.FloorToInt(_currentTime % 60);
        measurementData.timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void UpdateAttemptDisplay(int add)
    {
        attempt += add;
        measurementData.attempt.text = attempt.ToString();
    }

    public void UpdateMistakeDisplay(int add)
    {
        mistake += add;
        measurementData.mistake.text = mistake.ToString();
    }

    public void UpdateScoreDisplay()
    {
        int minAttempt = section.tasks.Count;
        int acceptedAttempt = minAttempt * 2;
        int diff = Mathf.Max(0, attempt - acceptedAttempt);
        var penaltyMultiplier = Mathf.Ceil(diff / (float)minAttempt);
        var subAttempt = (int)penaltyMultiplier * 5;

        int subMistake = mistake * 10;

        score -= subAttempt + subMistake;
        measurementData.score.text = score.ToString();
    }
}