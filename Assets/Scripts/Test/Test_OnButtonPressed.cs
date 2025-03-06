using UnityEngine;

public class Test_OnButtonPressed : MonoBehaviour
{
    public AudioSource audioSource;

    public void OnButtonPressed()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying) return;

        audioSource.Play();
    }
}