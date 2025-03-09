using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    public static VirtualWorldManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Photon Callback

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(
            "Player " + newPlayer.NickName + " is joining. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Bye-Bye!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Tutorial");
    }

    #endregion
}