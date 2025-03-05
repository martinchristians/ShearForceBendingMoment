using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public SceneReference sceneReference;
    public bool setActiveScene = true;

    private void Awake()
    {
        StartCoroutine(LoadAsynScene());
    }

    IEnumerator LoadAsynScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneReference.sceneName, LoadSceneMode.Additive);

        while (asyncLoad != null && !asyncLoad.isDone)
            yield return null;

        if (setActiveScene)
            SceneManager.SetActiveScene(sceneReference.scene);
    }
}