using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject GenericPlayerPrefab;
    public GameObject PlayerSpawnPosition;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (GenericPlayerPrefab != null && PlayerSpawnPosition != null)
                PhotonNetwork.Instantiate(GenericPlayerPrefab.name, PlayerSpawnPosition.transform.position,
                    PlayerSpawnPosition.transform.rotation);
        }
    }
}