public enum MeasurementValue
{
    ATTEMP,
    MISTAKE,
    SCORE
}

public class ComputeMeasurement : TriggerAction
{
    public MeasurementValue measurementValue;
    public int add;

    protected override void ExecuteTrigger()
    {
        switch (measurementValue)
        {
            case MeasurementValue.ATTEMP:
                SectionDataManager.instance.UpdateAttemptDisplay(add);
                break;
            case MeasurementValue.MISTAKE:
                SectionDataManager.instance.UpdateMistakeDisplay(add);
                break;
            case MeasurementValue.SCORE:
                SectionDataManager.instance.UpdateScoreDisplay();
                break;
        }
    }
}