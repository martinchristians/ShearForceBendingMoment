using Photon.Pun;
using UnityEngine;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public Test_MakeASound test;

    #region UI Callback

    public void ConnectAnonymously()
    {
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
    }

    #endregion
}