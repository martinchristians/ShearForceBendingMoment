using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject xrOriginGameObject;

    public GameObject avatarHeadGameObject;
    public GameObject avatarBodyGameObject;

    void Start()
    {
        if (photonView.IsMine)
        {
            xrOriginGameObject.SetActive(true);
            SetLayerRecursively(avatarHeadGameObject, 6);
            SetLayerRecursively(avatarBodyGameObject, 7);

            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if (teleportationAreas.Length > 0)
                Debug.Log("Found " + teleportationAreas.Length + " teleportation area(s).");

            foreach (var item in teleportationAreas)
                item.teleportationProvider = xrOriginGameObject.gameObject.GetComponent<TeleportationProvider>();
        }
        else
        {
            xrOriginGameObject.SetActive(false);
            SetLayerRecursively(avatarHeadGameObject, 0);
            SetLayerRecursively(avatarBodyGameObject, 0);
        }
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;

        foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = layerNumber;
    }
}