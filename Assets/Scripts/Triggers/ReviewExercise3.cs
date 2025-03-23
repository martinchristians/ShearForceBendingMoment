using System;
using UnityEngine;

public class ReviewExercise3 : ReviewExercise
{
    [SerializeField] private AttachableContainer[] attachableContainers;
    private AttachableContainer _beam;
    private BeamForces _beamForces;

    private void Awake()
    {
        _beam = attachableContainers[0];
        _beamForces = _beam.GetComponent<BeamForces>();
    }

    protected override void ReviewAttachment()
    {
        var taskCount = section.tasks.Count;
        var attachableObjectCount = _beam.attachedObjectInsideCollider.Count;
        if (taskCount != attachableObjectCount)
        {
            TriggerIncorrectAnswer();
            Debug.Log("Incorrect Answer! - Number attachable is wrong");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            var task = section.tasks[i];
            if (task is TaskPlacingOnBeam taskPlacingBeam)
            {
                var taskForce = taskPlacingBeam.WeightForce;

                var beamForceCalculator = _beamForces.beamForceCalculation;
                var beamForce = beamForceCalculator.forcesAndDistancesToStart[i].x;

                if (!Mathf.Approximately(taskForce, beamForce))
                {
                    TriggerIncorrectAnswer();
                    Debug.Log("Incorrect Answer! - Wrong type");
                    return;
                }

                var beamLength = beamForceCalculator.beamLength;
                var taskBeamPosition = beamLength * taskPlacingBeam.normalizedBeamPosition;
                var taskBeamRange = beamLength * taskPlacingBeam.normalizedBeamRange;

                var posAttachableObject = beamForceCalculator.forcesAndDistancesToStart[i].y;

                if (Math.Abs(taskBeamPosition - posAttachableObject) > taskBeamRange)
                {
                    TriggerIncorrectAnswer();
                    Debug.Log("Incorrect Answer!");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Incorrect task type!");
                return;
            }
        }

        TriggerCorrectAnswer(attachableContainers);
    }
}