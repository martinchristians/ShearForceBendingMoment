using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{
    //Avatar
    public Transform avatarTransform;
    public Transform avatarHead;
    public Transform avatarBody;
    public Transform avatarRightHand;
    public Transform avatarLeftHand;

    //XR Origin
    public Transform xrCamera;
    public Transform xrHandRight;
    public Transform xrHandLeft;
    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;

    private void Update()
    {
        if (avatarTransform == null) ReferencingAvatar();

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

    private void ReferencingAvatar()
    {
        avatarTransform = GameObject.FindGameObjectWithTag("Avatar").GetComponent<Transform>();
        avatarHead = GameObject.FindGameObjectWithTag("AvatarHead").GetComponent<Transform>();
        avatarBody = GameObject.FindGameObjectWithTag("AvatarBody").GetComponent<Transform>();
        avatarRightHand = GameObject.FindGameObjectWithTag("AvatarRightHand").GetComponent<Transform>();
        avatarLeftHand = GameObject.FindGameObjectWithTag("AvatarLeftHand").GetComponent<Transform>();
    }
}