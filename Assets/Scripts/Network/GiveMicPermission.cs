#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GiveMicPermission : TriggerAction
{
    protected override void ExecuteTrigger()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);
#endif
    }
}