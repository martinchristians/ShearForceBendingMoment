using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneReference", menuName = "Scenes/SceneReference")]
public class SceneReference : ScriptableObject
{
    public string sceneName;

    public Scene scene => SceneManager.GetSceneByName(sceneName);
}