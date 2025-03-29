using Photon.Pun;
using UnityEngine;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public static LoginManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    #region UI Callback

    public void ConnectAndLoadLobby()
    {
        var username = KeyboardManager.instance.username;
        PhotonNetwork.NickName = username == "" ? "NN" : username;

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