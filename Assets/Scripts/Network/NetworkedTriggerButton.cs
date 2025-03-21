using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedTriggerButton : MonoBehaviourPunCallbacks
{
    private Button _button;
    private PhotonView _photonView;

    [Header("Set Active GameObject")]
    [SerializeField] private GameObject[] setActive;
    [SerializeField] private GameObject[] setDeactive;

    [Header("Play Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animTrigger;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        if (_button)
            _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_photonView && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            _photonView.RPC("OnNetworkTrigger", RpcTarget.AllBuffered);
        }
        else
            OnLocalTrigger();
    }

    [PunRPC]
    private void OnNetworkTrigger()
    {
        OnLocalTrigger();
    }

    private void OnLocalTrigger()
    {
        if (setActive.Length != 0)
            SetActive();

        if (animator && animTrigger != "")
            PlayAnimation();

        if (setDeactive.Length != 0)
            SetDeactive();
    }

    private void SetActive()
    {
        foreach (var go in setActive)
            go.SetActive(true);
    }

    private void SetDeactive()
    {
        foreach (var go in setDeactive)
            go.SetActive(false);
    }

    private void PlayAnimation()
    {
        animator.SetTrigger(animTrigger);
    }
}