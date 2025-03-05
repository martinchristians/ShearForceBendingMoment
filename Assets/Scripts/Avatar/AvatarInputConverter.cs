using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{
    //Avatar transform
    public Transform mainAvatarTransform;
    public Transform avatarHead;
    public Transform avatarBody;
    public Transform avatarHandRight;
    public Transform avatarHandLeft;

    //XRRig transform
    public Transform XRHead;
    public Transform XRHandRight;
    public Transform XRHandLeft;
    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;

    private void Update()
    {
        //Sync head & body
        mainAvatarTransform.position =
            Vector3.Lerp(mainAvatarTransform.position, XRHead.position + headPositionOffset, 0.5f);
        avatarHead.rotation = Quaternion.Lerp(avatarHead.rotation, XRHead.rotation, 0.5f);
        avatarBody.rotation = Quaternion.Lerp(avatarBody.rotation,
            Quaternion.Euler(new Vector3(0, avatarHead.rotation.eulerAngles.y, 0)), 0.5f);

        //Hand synch
        avatarHandRight.position = Vector3.Lerp(avatarHandRight.position, XRHandRight.position, 0.5f);
        avatarHandRight.rotation = Quaternion.Lerp(avatarHandRight.rotation, XRHandRight.rotation, 0.5f) *
                                   Quaternion.Euler(handRotationOffset);
        avatarHandLeft.position = Vector3.Lerp(avatarHandLeft.position, XRHandLeft.position, 0.5f);
        avatarHandLeft.rotation = Quaternion.Lerp(avatarHandLeft.rotation, XRHandLeft.rotation, 0.5f) *
                                  Quaternion.Euler(handRotationOffset);
    }
}