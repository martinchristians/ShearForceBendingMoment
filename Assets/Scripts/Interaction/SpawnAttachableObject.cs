using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnAttachableObject : MonoBehaviourPun
{
    [SerializeField] private AttachableObjectType attachableObjectType;
    [SerializeField] private Transform spawnTarget;
    [SerializeField] private int waitSeconds = 1;
    [SerializeField] private Transform[] transformObjects;

    private PhotonView _photonView;
    private bool _isOccupied;

    [SerializeField] private List<TriggerAction> onSpawnTriggerActions;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerStay(Collider other)
    {
        var attachableObject = other.gameObject.GetComponent<AttachableObject>();
        if (!attachableObject) return;
        if (attachableObject.attachableObjectType != attachableObjectType) return;

        _isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        var attachableObject = other.gameObject.GetComponent<AttachableObject>();
        if (!attachableObject) return;
        if (attachableObject.attachableObjectType != attachableObjectType) return;

        _isOccupied = false;
        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            _photonView.RPC("RespawnNetworkedAttachableObject", RpcTarget.AllBuffered);
        }
        else
        {
            StartCoroutine(RespawnLocalAttachableObject());
        }
    }

    [PunRPC]
    private void RespawnNetworkedAttachableObject()
    {
        StartCoroutine(RespawnLocalAttachableObject());
    }

    private IEnumerator RespawnLocalAttachableObject()
    {
        if (transformObjects.Length == 0)
        {
            Debug.LogWarning("Assign the transformObjects array!");
            yield break;
        }

        yield return new WaitForSeconds(waitSeconds);

        if (_isOccupied) yield break;

        foreach (var t in transformObjects)
        {
            if (t.gameObject.layer == 11) continue;

            var ao = t.GetComponent<AttachableObject>();
            if (ao.isAttachableContainerFilled) continue;

            ao.transform.position = spawnTarget.position;
            ao.transform.rotation = spawnTarget.rotation;
            break;
        }

        onSpawnTriggerActions.ForEach(ta => ta.OnTrigger());
    }
}