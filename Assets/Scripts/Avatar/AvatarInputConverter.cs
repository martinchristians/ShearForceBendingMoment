using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{
    public Transform avatarTransform;
    public Transform avatarHead;
    public Transform avatarBody;
    public Transform avatarRightHand;
    public Transform avatarLeftHand;

    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;

    private bool _isVRPlayer;

    [Header("XR Player")] public Transform xrCamera;
    public Transform xrHandRight;
    public Transform xrHandLeft;

    [Header("Desktop Player")] public Transform desktopCamera;

    private void Start()
    {
        if (xrCamera)
            _isVRPlayer = true;
    }

    private void Update()
    {
        if (avatarTransform == null) ReferencingAvatar();

        if (_isVRPlayer)
        {
            //Avatar
            avatarTransform.position =
                Vector3.Lerp(avatarTransform.position, xrCamera.position + headPositionOffset, 0.5f);

            //AvatarHead
            avatarHead.rotation = Quaternion.Lerp(avatarHead.rotation, xrCamera.rotation, 0.5f);

            //AvatarBody
            avatarBody.rotation = Quaternion.Lerp(avatarBody.rotation,
                Quaternion.Euler(new Vector3(0, avatarHead.rotation.eulerAngles.y, 0)), 0.5f);

            //AvatarRightHand
            avatarRightHand.position = Vector3.Lerp(avatarRightHand.position, xrHandRight.position, 0.5f);
            avatarRightHand.rotation =
                Quaternion.Lerp(avatarRightHand.rotation, xrHandRight.rotation, 0.5f) *
                Quaternion.Euler(handRotationOffset);

            //AvatarLeftHand
            avatarLeftHand.position = Vector3.Lerp(avatarLeftHand.position, xrHandLeft.position, 0.5f);
            avatarLeftHand.rotation = Quaternion.Lerp(avatarLeftHand.rotation, xrHandLeft.rotation, 0.5f) *
                                      Quaternion.Euler(handRotationOffset);
        }
        else
        {
            //Avatar
            avatarTransform.position =
                Vector3.Lerp(avatarTransform.position, desktopCamera.position + headPositionOffset, 0.5f);
        }
    }

    private void ReferencingAvatar()
    {
        avatarTransform = GameObject.FindGameObjectWithTag("Avatar").GetComponent<Transform>();
        avatarHead = GameObject.FindGameObjectWithTag("AvatarHead").GetComponent<Transform>();
        avatarBody = GameObject.FindGameObjectWithTag("AvatarBody").GetComponent<Transform>();

        if (!_isVRPlayer) return;
        avatarRightHand = GameObject.FindGameObjectWithTag("AvatarRightHand").GetComponent<Transform>();
        avatarLeftHand = GameObject.FindGameObjectWithTag("AvatarLeftHand").GetComponent<Transform>();
    }
}