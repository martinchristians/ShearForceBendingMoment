using UnityEngine;

public class PlayParticle : TriggerAction
{
    [SerializeField] private ParticleSystem _particleSystem;

    protected override void ExecuteTrigger()
    {
        if (_particleSystem) _particleSystem.Play();
    }
}