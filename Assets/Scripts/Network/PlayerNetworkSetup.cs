using UnityEngine;
using Photon.Pun;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject avatar;

    public GameObject avatarHeadGameObject;
    public GameObject avatarBodyGameObject;

    public GameObject[] avatarHeadModelPrefabs;
    public GameObject[] avatarBodyModelPrefabs;

    void Start()
    {
        if (photonView.IsMine)
        {
            //Get avatar selection data
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(
                    MultiplayerVRConstants.AVATAR_HEAD_SELECTION_NUMBER, out var avatarHeadSelectionNumber))
            {
                Debug.Log("Avatar Head selection number: " + (int)avatarHeadSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarHeadModel", RpcTarget.AllBuffered,
                    (int)avatarHeadSelectionNumber);
            }

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(
                    MultiplayerVRConstants.AVATAR_BODY_SELECTION_NUMBER, out var avatarBodySelectionNumber))
            {
                Debug.Log("Avatar Body selection number: " + (int)avatarBodySelectionNumber);
                photonView.RPC("InitializeSelectedAvatarBodyModel", RpcTarget.AllBuffered,
                    (int)avatarBodySelectionNumber);
            }

            avatar.AddComponent<AudioListener>();

            SetLayerRecursively(avatarHeadGameObject, 6);
            SetLayerRecursively(avatarBodyGameObject, 7);
        }
        else
        {
            SetLayerRecursively(avatarHeadGameObject, 0);
            SetLayerRecursively(avatarBodyGameObject, 0);
        }
    }

    public void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = layerNumber;
    }

    [PunRPC]
    public void InitializeSelectedAvatarHeadModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarHeadGameObject =
            Instantiate(avatarHeadModelPrefabs[avatarSelectionNumber], avatarHeadGameObject.transform);

        selectedAvatarHeadGameObject.GetComponent<Transform>().localPosition = Vector3.zero;
        selectedAvatarHeadGameObject.GetComponent<Transform>().localRotation = Quaternion.identity;
        selectedAvatarHeadGameObject.GetComponent<Transform>().localScale = Vector3.one;
    }

    [PunRPC]
    public void InitializeSelectedAvatarBodyModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarBodyGameObject =
            Instantiate(avatarBodyModelPrefabs[avatarSelectionNumber], avatarBodyGameObject.transform);

        selectedAvatarBodyGameObject.GetComponent<Transform>().localPosition = Vector3.zero;
        selectedAvatarBodyGameObject.GetComponent<Transform>().localRotation = Quaternion.identity;
        selectedAvatarBodyGameObject.GetComponent<Transform>().localScale = Vector3.one;
    }
}