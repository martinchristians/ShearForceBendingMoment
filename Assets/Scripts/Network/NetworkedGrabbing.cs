using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private PhotonView _photonView;

    private Rigidbody _rb;
    private bool _isObjectBeingHeld;

    //For desktop player
    private Transform _playerCamera;
    private float _grabDistance;

    [SerializeField] private Transform attachTransform;
    private Vector3 _grabOffset;

    const float LerpSpeed = 10f;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isObjectBeingHeld)
        {
            _rb.isKinematic = true;
            gameObject.layer = 11;

            //For desktop player
            if (!_playerCamera) return;

            Vector3 targetPosition = _playerCamera.position + _playerCamera.forward * _grabDistance;
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition + _grabOffset,
                Time.deltaTime * LerpSpeed);
            _rb.MovePosition(newPos);

            Quaternion targetRotation = Quaternion.LookRotation(_playerCamera.position - transform.position);
            Quaternion newRotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LerpSpeed);
            _rb.MoveRotation(newRotation);
        }
        else
        {
            _rb.isKinematic = false;
            gameObject.layer = 9;
        }
    }

    private void TransferOwnership()
    {
        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            _photonView.RequestOwnership();
    }

    public void OnSelectEntered(Transform playerCamera)
    {
        if (_isObjectBeingHeld) return;

        Debug.Log(gameObject.name + " is grabbed!");

        if (playerCamera) _playerCamera = playerCamera;

        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            _photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);

            if (_photonView.Owner == PhotonNetwork.LocalPlayer)
                Debug.Log("Ownership remains the same!");
            else
                TransferOwnership();
        }
        else
            StartLocalGrabbing();
    }

    public void OnSelectExited()
    {
        Debug.Log(gameObject.name + " is released!");

        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            _photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
        else
            StopLocalGrabbing();
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

        //For desktop player
        if (_playerCamera == null) return;
        _grabDistance = Vector3.Distance(transform.position, _playerCamera.position);
        _grabOffset = transform.position - attachTransform.position;
        _rb.useGravity = false;
        _rb.freezeRotation = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        _isObjectBeingHeld = false;

        //For desktop player
        if (_playerCamera == null) return;
        _playerCamera = null;
        _rb.useGravity = true;
        _rb.freezeRotation = false;
    }

    private void StartLocalGrabbing()
    {
        _isObjectBeingHeld = true;

        //For desktop player
        if (_playerCamera == null) return;
        _grabDistance = Vector3.Distance(transform.position, _playerCamera.position);
        _grabOffset = transform.position - attachTransform.position;
        _rb.useGravity = false;
        _rb.freezeRotation = true;
    }

    private void StopLocalGrabbing()
    {
        _isObjectBeingHeld = false;

        //For desktop player
        if (_playerCamera == null) return;
        _playerCamera = null;
        _rb.useGravity = true;
        _rb.freezeRotation = false;
    }
}