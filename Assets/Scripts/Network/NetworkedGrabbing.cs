using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private Rigidbody _rb;
    private PhotonView _photonView;
    private AttachableObject _attachableObject;

    private bool _isObjectBeingHeld;

    //For desktop player
    private Transform _playerCamera;
    private float _grabDistance;

    [SerializeField] private Transform transformPosAttachRaycastOffset_Desktop;
    private Vector3 _grabOffset;

    const float LerpSpeed = 10f;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _attachableObject = GetComponent<AttachableObject>();
    }

    private void FixedUpdate()
    {
        if (!_isObjectBeingHeld) return;

        //For desktop player
        if (_playerCamera)
        {
            Vector3 targetPosition = _playerCamera.position + _playerCamera.forward * _grabDistance;
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition + _grabOffset,
                Time.deltaTime * LerpSpeed);
            _rb.MovePosition(newPos);

            Quaternion targetRotation = Quaternion.LookRotation(_playerCamera.position - transform.position);

            Quaternion newRotation =
                Quaternion.Slerp(transform.rotation, targetRotation,
                    Time.deltaTime * LerpSpeed);
            _rb.MoveRotation(newRotation);
        }
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
        {
            _photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
        }
        else
            StopLocalGrabbing();
    }

    private void TransferOwnership()
    {
        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            _photonView.RequestOwnership();
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
        StartLocalGrabbing();
    }

    private void StartLocalGrabbing()
    {
        _isObjectBeingHeld = true;
        gameObject.layer = 11;

        _rb.isKinematic = true;

        //For desktop player
        if (_playerCamera)
        {
            _grabDistance = Vector3.Distance(transform.position, _playerCamera.position);
            _grabOffset = transform.position - transformPosAttachRaycastOffset_Desktop.position;

            _rb.useGravity = false;
            _rb.freezeRotation = true;
        }
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        StopLocalGrabbing();
    }

    private void StopLocalGrabbing()
    {
        _isObjectBeingHeld = false;
        gameObject.layer = 9;

        CheckIfReleasedOnAttachableContainer();

        //For desktop player
        if (_playerCamera)
        {
            _playerCamera = null;
        }
    }

    private void CheckIfReleasedOnAttachableContainer()
    {
        if (!_attachableObject)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.freezeRotation = false;
            return;
        }

        if (!_attachableObject.isAttachableContainersFilled)
        {
            if (_attachableObject.isAttachableContainerFilled)
            {
                Debug.Log("Released and detached");
                _attachableObject.Detach();
            }
            else
            {
                Debug.Log("Released outside container");
            }

            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.freezeRotation = false;
        }
        else
        {
            Debug.Log("Released inside container");
            _attachableObject.Attach();

            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.freezeRotation = true;
        }
    }
}