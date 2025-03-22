using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedTriggerButton : MonoBehaviourPunCallbacks
{
    private Button _button;
    private PhotonView _photonView;

    [SerializeField] private List<TriggerAction> triggerActions = new();

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        if (_button)
            _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            _photonView.RPC("OnNetworkTrigger", RpcTarget.AllBuffered);
        }
        else
            OnLocalTrigger();
    }

    [PunRPC]
    private void OnNetworkTrigger()
    {
        OnLocalTrigger();
    }

    private void OnLocalTrigger()
    {
        triggerActions.ForEach(ta => ta?.OnTrigger());
    }
}