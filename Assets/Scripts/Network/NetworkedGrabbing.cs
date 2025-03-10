using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private PhotonView _photonView;

    private Rigidbody _rb;
    private bool _isObjectBeingHeld;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isObjectBeingHeld)
        {
            _rb.isKinematic = true;
            gameObject.layer = 11;
        }
        else
        {
            _rb.isKinematic = false;
            gameObject.layer = 9;
        }
    }

    private void TransferOwnership()
    {
        _photonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        Debug.Log(gameObject.name + " is grabbed!");

        _photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);

        if (_photonView.Owner == PhotonNetwork.LocalPlayer)
            Debug.Log("Ownership remains the same!");
        else
            TransferOwnership();
    }

    public void OnSelectExited()
    {
        Debug.Log(gameObject.name + " is released!");

        _photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != _photonView) return;

        Debug.Log("... Player-" + requestingPlayer.NickName + " requests ownership for object-" + targetView.name);
        _photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("... Ownership is transferred to" + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("... Failed to transfer ownership!");
    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        _isObjectBeingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        _isObjectBeingHeld = false;
    }
}