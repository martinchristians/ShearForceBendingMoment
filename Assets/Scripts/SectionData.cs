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
    
    public InfoHintData infoHintData;
    public List<HintData> hintDataList;
    
    public MeasurementData measurementData;
    [SerializeField] private float startTime = 180f;
    [HideInInspector] public bool isStartTimer;
    private float _currentTime;

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
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);
        measurementData.timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}