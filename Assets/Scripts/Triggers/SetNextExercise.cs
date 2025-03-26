using UnityEngine;

public class SetNextExercise : TriggerAction
{
    public Session nextSession;

    protected override void ExecuteTrigger()
    {
        if (nextSession)
        {
            SessionDataManager.instance.activeSession = nextSession;
            SessionDataManager.instance.activeSection = nextSession.sections[0];

            var activeSection = SessionDataManager.instance.activeSection;
            for (int i = 0; i < SessionDataManager.instance.sections.Length; i++)
            {
                var section = SessionDataManager.instance.sections[i];
                if (section.Equals(activeSection))
                    SessionDataManager.instance.activeHintData = SessionDataManager.instance.infoHintDatas[i];
            }
        }
        else
        {
            var activeSession = SessionDataManager.instance.activeSession;
            var activeSection = SessionDataManager.instance.activeSection;
            var intNextSection = activeSection.sectionIndex + 1;

            var next = activeSession.sections[intNextSection];
            if (next)
                SessionDataManager.instance.activeSection = next;
            else
                Debug.Log("Last section.. Please assign the next session!");
        }

        SectionDataManager.instance.SetInitValue();
    }
}