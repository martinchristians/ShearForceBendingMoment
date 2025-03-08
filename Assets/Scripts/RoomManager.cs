using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string _mapType;
    public int maxPlayer;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region UI Callback

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinExerciseRoom()
    {
        _mapType = MultiplayerVRConstants.MapTypeValueExercise;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MapTypeKey, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
    }

    public void JoinExperimentRoom()
    {
        _mapType = MultiplayerVRConstants.MapTypeValueExperiment;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MapTypeKey, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
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

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MapTypeKey))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MapTypeKey, out mapType))
            {
                Debug.Log("Joined room with type: " + (string)mapType);

                switch ((string)mapType)
                {
                    case MultiplayerVRConstants.MapTypeValueExercise:
                        PhotonNetwork.LoadLevel("Exercise");
                        break;
                    case MultiplayerVRConstants.MapTypeValueExperiment:
                        PhotonNetwork.LoadLevel("Experiment");
                        break;
                }
            }
        }
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
        roomOptions.MaxPlayers = maxPlayer;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MapTypeKey };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MapTypeKey, _mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
}