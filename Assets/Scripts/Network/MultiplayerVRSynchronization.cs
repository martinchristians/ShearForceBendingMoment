using UnityEngine;
using Photon.Pun;

public class MultiplayerVRSynchronization : MonoBehaviour, IPunObservable
{
    private PhotonView _photonView;
    private bool _firstTake;

    //Local VR player
    public Transform genericPlayer;

    private float _distanceGenericPlayer;
    private Vector3 _directionGenericPlayer;
    private Vector3 _networkPositionGenericPlayer;
    private Vector3 _storedPositionGenericPlayer;

    private float _angleGenericPlayer;
    private Quaternion _networkRotationGenericPlayer;

    //Avatar
    public Transform avatar;

    private float _distanceAvatar;
    private Vector3 _directionAvatar;
    private Vector3 _networkPositionAvatar;
    private Vector3 _storedPositionAvatar;

    private float _angleAvatar;
    private Quaternion _networkRotationAvatar;

    //Avatar Head
    public Transform avatarHead;

    private float _angleAvatarHead;
    private Quaternion _networkRotationAvatarHead;

    //Avatar Body
    public Transform avatarBody;

    private float _angleAvatarBody;
    private Quaternion _networkRotationAvatarBody;

    //Avatar LeftHand
    public Transform avatarLeftHand;

    private float _distanceAvatarLeftHand;
    private Vector3 _directionAvatarLeftHand;
    private Vector3 _networkPositionAvatarLeftHand;
    private Vector3 _storedPositionAvatarLeftHand;

    private float _angleAvatarLeftHand;
    private Quaternion _networkRotationAvatarLeftHand;

    //Avatar RightHand
    public Transform avatarRightHand;

    private float _distanceAvatarRightHand;
    private Vector3 _directionAvatarRightHand;
    private Vector3 _networkPositionAvatarRightHand;
    private Vector3 _storedPositionAvatarRightHand;

    private float _angleAvatarRightHand;
    private Quaternion _networkRotationAvatarRightHand;

    public void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        //Local VR player
        _storedPositionGenericPlayer = genericPlayer.position;
        _networkPositionGenericPlayer = Vector3.zero;

        _networkRotationGenericPlayer = Quaternion.identity;

        //Avatar
        _storedPositionAvatar = avatar.localPosition;
        _networkPositionAvatar = Vector3.zero;

        _networkRotationAvatar = Quaternion.identity;

        //Avatar Head
        _networkRotationAvatarHead = Quaternion.identity;

        //Avatar Body
        _networkRotationAvatarBody = Quaternion.identity;

        //Avatar LeftHand
        _storedPositionAvatarLeftHand = avatarLeftHand.localPosition;
        _networkPositionAvatarLeftHand = Vector3.zero;

        _networkRotationAvatarLeftHand = Quaternion.identity;

        //Avatar RightHand
        _storedPositionAvatarRightHand = avatarRightHand.localPosition;
        _networkPositionAvatarRightHand = Vector3.zero;

        _networkRotationAvatarRightHand = Quaternion.identity;
    }

    void OnEnable()
    {
        _firstTake = true;
    }

    public void Update()
    {
        if (_photonView.IsMine) return;

        //Local VR player
        genericPlayer.position = Vector3.MoveTowards(
            genericPlayer.position,
            _networkPositionGenericPlayer,
            _distanceGenericPlayer * (1.0f / PhotonNetwork.SerializationRate));

        genericPlayer.rotation = Quaternion.RotateTowards(
            genericPlayer.rotation,
            _networkRotationGenericPlayer,
            _angleGenericPlayer * (1.0f / PhotonNetwork.SerializationRate));

        //Avatar
        avatar.localPosition = Vector3.MoveTowards(
            avatar.localPosition,
            _networkPositionAvatar,
            _distanceAvatar * (1.0f / PhotonNetwork.SerializationRate));

        avatar.localRotation = Quaternion.RotateTowards(
            avatar.localRotation,
            _networkRotationAvatar,
            _angleAvatar * (1.0f / PhotonNetwork.SerializationRate));

        //Avatar Head
        avatarHead.localRotation = Quaternion.RotateTowards(
            avatarHead.localRotation,
            _networkRotationAvatarHead,
            _angleAvatarHead * (1.0f / PhotonNetwork.SerializationRate));

        //Avatar Body
        avatarBody.localRotation = Quaternion.RotateTowards(
            avatarBody.localRotation,
            _networkRotationAvatarBody,
            _angleAvatarBody * (1.0f / PhotonNetwork.SerializationRate));

        //Avatar LeftHand
        avatarLeftHand.localPosition = Vector3.MoveTowards(
            avatarLeftHand.localPosition,
            _networkPositionAvatarLeftHand,
            _distanceAvatarLeftHand * (1.0f / PhotonNetwork.SerializationRate));

        avatarLeftHand.localRotation = Quaternion.RotateTowards(
            avatarLeftHand.localRotation,
            _networkRotationAvatarLeftHand,
            _angleAvatarLeftHand * (1.0f / PhotonNetwork.SerializationRate));

        //Avatar RightHand
        avatarRightHand.localPosition = Vector3.MoveTowards(
            avatarRightHand.localPosition,
            _networkPositionAvatarRightHand,
            _distanceAvatarRightHand * (1.0f / PhotonNetwork.SerializationRate));

        avatarRightHand.localRotation = Quaternion.RotateTowards(
            avatarRightHand.localRotation,
            _networkRotationAvatarRightHand,
            _angleAvatarRightHand * (1.0f / PhotonNetwork.SerializationRate));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Local VR player
            _directionGenericPlayer = genericPlayer.position - _storedPositionGenericPlayer;
            _storedPositionGenericPlayer = genericPlayer.position;
            stream.SendNext(genericPlayer.position);
            stream.SendNext(_directionGenericPlayer);

            stream.SendNext(genericPlayer.rotation);

            //Avatar
            _directionAvatar = avatar.localPosition - _storedPositionAvatar;
            _storedPositionAvatar = avatar.localPosition;
            stream.SendNext(avatar.localPosition);
            stream.SendNext(_directionAvatar);

            stream.SendNext(avatar.localRotation);

            //Avatar Head
            stream.SendNext(avatarHead.localRotation);

            //Avatar Body
            stream.SendNext(avatarBody.localRotation);

            //Avatar LeftHand
            _directionAvatarLeftHand = avatarLeftHand.localPosition - _storedPositionAvatarLeftHand;
            _storedPositionAvatarLeftHand = avatarLeftHand.localPosition;
            stream.SendNext(avatarLeftHand.localPosition);
            stream.SendNext(_directionAvatarLeftHand);

            stream.SendNext(avatarLeftHand.localRotation);

            //Avatar RightHand
            _directionAvatarRightHand = avatarRightHand.localPosition - _storedPositionAvatarRightHand;
            _storedPositionAvatarRightHand = avatarRightHand.localPosition;
            stream.SendNext(avatarRightHand.localPosition);
            stream.SendNext(_directionAvatarRightHand);

            stream.SendNext(avatarRightHand.localRotation);
        }
        else
        {
            //Local VR player - position
            _networkPositionGenericPlayer = (Vector3)stream.ReceiveNext();
            _directionGenericPlayer = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                genericPlayer.position = _networkPositionGenericPlayer;
                _distanceGenericPlayer = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPositionGenericPlayer += _directionGenericPlayer * lag;
                _distanceGenericPlayer =
                    Vector3.Distance(genericPlayer.position, _networkPositionGenericPlayer);
            }

            //Local VR player - rotation
            _networkRotationGenericPlayer = (Quaternion)stream.ReceiveNext();
            if (_firstTake)
            {
                _angleGenericPlayer = 0f;
                genericPlayer.rotation = _networkRotationGenericPlayer;
            }
            else
                _angleGenericPlayer =
                    Quaternion.Angle(genericPlayer.rotation, _networkRotationGenericPlayer);

            //Avatar - position
            _networkPositionAvatar = (Vector3)stream.ReceiveNext();
            _directionAvatar = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                avatar.localPosition = _networkPositionAvatar;
                _distanceAvatar = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPositionAvatar += _directionAvatar * lag;
                _distanceAvatar =
                    Vector3.Distance(avatar.localPosition, _networkPositionAvatar);
            }

            //Avatar - rotation
            _networkRotationAvatar = (Quaternion)stream.ReceiveNext();
            if (_firstTake)
            {
                _angleAvatar = 0f;
                avatar.rotation = _networkRotationAvatar;
            }
            else
                _angleAvatar =
                    Quaternion.Angle(avatar.rotation, _networkRotationAvatar);

            //Avatar Head - rotation
            _networkRotationAvatarHead = (Quaternion)stream.ReceiveNext();

            if (_firstTake)
            {
                _angleAvatarHead = 0f;
                avatarHead.localRotation = _networkRotationAvatarHead;
            }
            else
                _angleAvatarHead =
                    Quaternion.Angle(avatarHead.localRotation, _networkRotationAvatarHead);

            //Avatar Body - rotation
            _networkRotationAvatarBody = (Quaternion)stream.ReceiveNext();

            if (_firstTake)
            {
                _angleAvatarBody = 0f;
                avatarBody.localRotation = _networkRotationAvatarBody;
            }
            else
                _angleAvatarBody =
                    Quaternion.Angle(avatarBody.localRotation, _networkRotationAvatarBody);


            //Avatar LeftHand - position
            _networkPositionAvatarLeftHand = (Vector3)stream.ReceiveNext();
            _directionAvatarLeftHand = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                avatarLeftHand.localPosition = _networkPositionAvatarLeftHand;
                _distanceAvatarLeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPositionAvatarLeftHand += _directionAvatarLeftHand * lag;
                _distanceAvatarLeftHand =
                    Vector3.Distance(avatarLeftHand.localPosition, _networkPositionAvatarLeftHand);
            }

            //Avatar LeftHand - rotation
            _networkRotationAvatarLeftHand = (Quaternion)stream.ReceiveNext();
            if (_firstTake)
            {
                _angleAvatarLeftHand = 0f;
                avatarLeftHand.localRotation = _networkRotationAvatarLeftHand;
            }
            else
                _angleAvatarLeftHand =
                    Quaternion.Angle(avatarLeftHand.localRotation, _networkRotationAvatarLeftHand);

            //Avatar RightHand - position
            _networkPositionAvatarRightHand = (Vector3)stream.ReceiveNext();
            _directionAvatarRightHand = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                avatarRightHand.localPosition = _networkPositionAvatarRightHand;
                _distanceAvatarRightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPositionAvatarRightHand += _directionAvatarRightHand * lag;
                _distanceAvatarRightHand =
                    Vector3.Distance(avatarRightHand.localPosition, _networkPositionAvatarRightHand);
            }

            //Avatar RightHand - rotation
            _networkRotationAvatarRightHand = (Quaternion)stream.ReceiveNext();
            if (_firstTake)
            {
                _angleAvatarRightHand = 0f;
                avatarRightHand.localRotation = _networkRotationAvatarRightHand;
            }
            else
                _angleAvatarRightHand =
                    Quaternion.Angle(avatarRightHand.localRotation, _networkRotationAvatarRightHand);

            if (_firstTake)
                _firstTake = false;
        }
    }
}