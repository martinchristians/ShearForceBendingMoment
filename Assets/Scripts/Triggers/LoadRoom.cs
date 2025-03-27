using UnityEngine;

public class LoadRoom : TriggerAction
{
    public LevelScene LevelScene;

    protected override void ExecuteTrigger()
    {
        switch (LevelScene)
        {
            case LevelScene.NONE:
                Debug.Log("No scene is assigned!");
                break;
            case LevelScene.EXERCISE1:
                RoomManager.instance.JoinExercise1Room();
                break;
            case LevelScene.EXERCISE2:
                RoomManager.instance.JoinExercise2Room();
                break;
            case LevelScene.EXERCISE3:
                RoomManager.instance.JoinExercise3Room();
                break;
            case LevelScene.EXPERIMENT:
                RoomManager.instance.JoinExperimentRoom();
                break;
        }
    }
}