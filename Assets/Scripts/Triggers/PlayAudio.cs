using UnityEngine;

public class PlayAudio : TriggerAction
{
    public AudioSource audioSource;

    protected override void ExecuteTrigger()
    {
        if (!audioSource)
        {
            Debug.Log("PlayAudio TriggerAction isn't executed!");
            return;
        }

        audioSource.Play();
    }
}