using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region UI Callback

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region Photon Callback

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A new room " + PhotonNetwork.CurrentRoom.Name + " is created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player " + PhotonNetwork.NickName + " is joining " + PhotonNetwork.CurrentRoom.Name +
                  "... Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(
            "Player " + newPlayer.NickName + " is joining. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "ROOM-" + Random.Range(0, 100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
}