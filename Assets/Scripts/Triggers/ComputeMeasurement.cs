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
                SectionData.instance.UpdateAttemptDisplay(add);
                break;
            case MeasurementValue.MISTAKE:
                SectionData.instance.UpdateMistakeDisplay(add);
                break;
            case MeasurementValue.SCORE:
                SectionData.instance.UpdateScoreDisplay();
                break;
        }
    }
}