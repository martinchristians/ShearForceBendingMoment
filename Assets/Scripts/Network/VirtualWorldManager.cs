using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<TriggerAction> triggerActionsOnEnteredRoom = new();
    [SerializeField] private List<TriggerAction> triggerActionsOnLeftRoom = new();
    [SerializeField] private int delayBeforeLeft;

    #region Photon Callback

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(
            "Player " + newPlayer.NickName + " is joining. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (triggerActionsOnEnteredRoom.Count > 0)
            triggerActionsOnEnteredRoom.ForEach(ta => ta.OnTrigger());
    }

    public override void OnLeftRoom()
    {
        StartCoroutine(LoadLobby());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Tutorial");
    }

    #endregion

    private IEnumerator LoadLobby()
    {
        if (triggerActionsOnLeftRoom.Count > 0)
            triggerActionsOnLeftRoom.ForEach(ta => ta.OnTrigger());

        yield return new WaitForSeconds(delayBeforeLeft);

        PhotonNetwork.Disconnect();
        Debug.Log("Bye-Bye!");
    }
}