using UnityEngine;

public class Test_MakeASound : MonoBehaviour
{
    public AudioSource audioSource;

    public void MakeASound()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying) return;

        audioSource.Play();
    }
}