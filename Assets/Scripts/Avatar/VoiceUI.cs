using UnityEngine;
using TMPro;
using Photon.Voice.PUN;

public class VoiceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI voiceState;

    private PhotonVoiceNetwork _punVoiceNetwork;

    private void Awake()
    {
        _punVoiceNetwork = PhotonVoiceNetwork.Instance;
    }

    private void OnEnable()
    {
        _punVoiceNetwork.Client.StateChanged += VoiceClientStateChanged;
    }

    private void OnDisable()
    {
        _punVoiceNetwork.Client.StateChanged -= VoiceClientStateChanged;
    }

    private void Update()
    {
        if (_punVoiceNetwork == null)
            _punVoiceNetwork = PhotonVoiceNetwork.Instance;
    }


    private void VoiceClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
    {
        UpdateUiBasedOnVoiceState(toState);
    }

    private void UpdateUiBasedOnVoiceState(Photon.Realtime.ClientState voiceClientState)
    {
        voiceState.text = string.Format("PhotonVoice: {0}", voiceClientState);
        if (voiceClientState == Photon.Realtime.ClientState.Joined)
            voiceState.gameObject.SetActive(false);
    }
}