using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Random = UnityEngine.Random;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string _mapType;
    public byte maxPlayer;

    public TextMeshProUGUI occupancyRateTextExercise1;
    public TextMeshProUGUI occupancyRateTextExercise2;
    public TextMeshProUGUI occupancyRateTextExercise3;
    public TextMeshProUGUI occupancyRateTextExperiment;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.ConnectUsingSettings();
        else
            PhotonNetwork.JoinLobby();
    }

    #region UI Callback

    public void JoinExercise1Room()
    {
        _mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE1;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MAP_TYPE_KEY, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
    }

    public void JoinExercise2Room()
    {
        _mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE2;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MAP_TYPE_KEY, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
    }

    public void JoinExercise3Room()
    {
        _mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE3;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MAP_TYPE_KEY, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
    }

    public void JoinExperimentRoom()
    {
        _mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_EXPERIMENT;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MAP_TYPE_KEY, _mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayer);
    }

    #endregion

    #region Photon Callback

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        CreateAndJoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Reconnecting to server...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A new room " + PhotonNetwork.CurrentRoom.Name + " is created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player " + PhotonNetwork.NickName + " is joining " + PhotonNetwork.CurrentRoom.Name +
                  "... Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY,
                    out mapType))
            {
                Debug.Log("Joined room with type: " + (string)mapType);

                switch ((string)mapType)
                {
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE1:
                        PhotonNetwork.LoadLevel("Exercise1");
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE2:
                        PhotonNetwork.LoadLevel("Exercise2");
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE3:
                        PhotonNetwork.LoadLevel("Exercise3");
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXPERIMENT:
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            occupancyRateTextExercise1.text = 0 + "/" + 20;
            occupancyRateTextExercise2.text = 0 + "/" + 20;
            occupancyRateTextExercise3.text = 0 + "/" + 20;
            occupancyRateTextExperiment.text = 0 + "/" + 20;
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE1))
                occupancyRateTextExercise1.text = room.PlayerCount + "/" + 20;
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE2))
                occupancyRateTextExercise2.text = room.PlayerCount + "/" + 20;
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE3))
                occupancyRateTextExercise3.text = room.PlayerCount + "/" + 20;
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_EXPERIMENT))
                occupancyRateTextExperiment.text = room.PlayerCount + "/" + 20;

            Debug.Log("Room: " + room.Name + " is filled with " + room.PlayerCount + " player.");
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("YEY.. Joined the lobby!");
    }

    #endregion

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "ROOM-" + _mapType + Random.Range(0, 100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.MAP_TYPE_KEY, _mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
}