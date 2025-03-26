using UnityEngine;

public class SetNextExercise : TriggerAction
{
    public Session nextSession;

    protected override void ExecuteTrigger()
    {
        SectionDataManager.instance.StoreSectionData();

        if (nextSession)
        {
            SessionDataManager.instance.activeSession = nextSession;
            SessionDataManager.instance.activeSection = nextSession.sections[0];

            SetActiveHintData(nextSession, nextSession.sections[0]);

            SectionDataManager.instance.SetRestoreValueFalse();
        }
        else
        {
            var activeSession = SessionDataManager.instance.activeSession;
            var activeSection = SessionDataManager.instance.activeSection;
            var intNextSection = activeSection.sectionIndex + 1;

            var next = activeSession.sections[intNextSection - 1];
            if (next)
                SessionDataManager.instance.activeSection = next;
            else
                Debug.Log("Last section.. Please assign the next session!");

            SetActiveHintData(activeSession, next);
        }

        SectionDataManager.instance.SetInitValue();
    }

    private void SetActiveHintData(Session activeSession, Section activeSection)
    {
        var numSectionsInSession1 = SessionDataManager.instance.sessions[0].sections.Count;
        var numSectionsInSession2 = SessionDataManager.instance.sessions[1].sections.Count;

        int initIndexHintData = 0;
        switch (activeSession.sessionIndex)
        {
            case 1:
                initIndexHintData = 0;
                break;
            case 2:
                initIndexHintData = numSectionsInSession1;
                break;
            case 3:
                initIndexHintData = numSectionsInSession1 + numSectionsInSession2;
                break;
        }

        Debug.Log("initIndexHintData" + initIndexHintData);

        foreach (var section in activeSession.sections)
        {
            if (section.Equals(activeSection))
            {
                var sectionGlobalIndex = section.sectionIndex + initIndexHintData - 1;
                SessionDataManager.instance.activeHintData =
                    SessionDataManager.instance.infoHintDatas[sectionGlobalIndex];
            }
        }
    }
}