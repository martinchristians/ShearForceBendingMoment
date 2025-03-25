using Photon.Pun;

public class BackLobby : TriggerAction
{
    protected override void ExecuteTrigger()
    {
        PhotonNetwork.LeaveRoom();
    }
}