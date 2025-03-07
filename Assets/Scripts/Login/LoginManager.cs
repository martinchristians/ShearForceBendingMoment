using Photon.Pun;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerInputName;

    public Test_MakeASound test;

    #region UI Callback

    public void ConnectAnonymously()
    {
        PhotonNetwork.NickName = "1";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectWithName()
    {
        PhotonNetwork.NickName = playerInputName != null ? playerInputName.text : "1";

        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Photon Callbak

    public override void OnConnected()
    {
        Debug.Log("... Server is available.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
        test.MakeASound();
        
        PhotonNetwork.LoadLevel("Exercise");
    }

    #endregion
}