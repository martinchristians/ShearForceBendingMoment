public class Login : TriggerAction
{
    protected override void ExecuteTrigger()
    {
        LoginManager.instance.ConnectAndLoadLobby();
    }
}