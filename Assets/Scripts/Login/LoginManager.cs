using Photon.Pun;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerInputName;

    #region UI Callback

    public void ConnectAnonymously()
    {
        PhotonNetwork.NickName = "1";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectWithName()
    {
        PhotonNetwork.NickName = playerInputName.text == "" ? "1" : playerInputName.text;

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
        Debug.Log("Connected! Player name: " + PhotonNetwork.NickName);

        PhotonNetwork.LoadLevel("Tutorial");
    }

    #endregion
}