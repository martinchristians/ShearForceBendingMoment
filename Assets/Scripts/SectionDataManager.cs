using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SectionDataInfo
{
    public Session session;
    public Section section;
    public bool isRestoreValue;

    public bool isTaskDone;
    public int minutes;
    public int seconds;
    public int attempt;
    public int mistake;
    public int score;
}

[System.Serializable]
public class HintData
{
    public string infoText;
    public bool wasShown;
    public List<TriggerAction> triggerList;
}

public class SectionDataManager : MonoBehaviour
{
    public TaskData taskData;
    public bool _isTaskDone;

    public MeasurementData measurementData;
    [SerializeField] private float startTime = 180f;
    private float _currentTime;
    [HideInInspector] public bool isStartTimer;
    public int minutes;
    public int seconds;
    public int attempt;
    public int mistake;
    public int score = 100;

    public List<HintData> hintDataList;

    [Header("Stored Data")] public List<SectionDataInfo> storedSectionDatas = new();

    public static SectionDataManager instance;

    private void Awake()
    {
        SetInitValue();

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

    public void SetInitValue()
    {
        taskData = null;
        _isTaskDone = false;

        measurementData = null;
        _currentTime = startTime;

        minutes = 0;
        seconds = 0;
        attempt = 0;
        mistake = 0;
        score = 100;

        hintDataList = null;
    }

    public void UpdateTaskState()
    {
        _isTaskDone = true;
        taskData.doneTask.gameObject.SetActive(true);
        taskData.undoneTask.gameObject.SetActive(false);
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
        int minAttempt = SessionDataManager.instance.activeSection.tasks.Count;
        int acceptedAttempt = minAttempt * 2;
        int diff = Mathf.Max(0, attempt - acceptedAttempt);
        var penaltyMultiplier = Mathf.Ceil(diff / (float)minAttempt);
        var subAttempt = (int)penaltyMultiplier * 5;

        int subMistake = mistake * 10;

        score -= subAttempt + subMistake;
        measurementData.score.text = score.ToString();
    }

    public void StoreSectionData()
    {
        SectionDataInfo newSectionDataInfo = new()
        {
            session = SessionDataManager.instance.activeSession,
            section = SessionDataManager.instance.activeSection,
            isRestoreValue = true,

            isTaskDone = _isTaskDone,
            minutes = minutes,
            seconds = seconds,
            attempt = attempt,
            mistake = mistake,
            score = score
        };

        storedSectionDatas.Add(newSectionDataInfo);
    }

    public void SetRestoreValueFalse()
    {
        storedSectionDatas.ForEach(data => data.isRestoreValue = false);
    }
}