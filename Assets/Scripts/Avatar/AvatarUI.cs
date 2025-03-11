using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;

public class AvatarUI : MonoBehaviour
{
    public GameObject voiceUIPrefab;
    private Image _micImage;
    [SerializeField] private Image speakerImage;
    [SerializeField] private PhotonVoiceView photonVoiceView;

    private void Awake()
    {
        speakerImage.enabled = false;
    }

    void Update()
    {
        if (_micImage == null)
        {
            var mainCamera = GameObject.FindWithTag("MainCamera");
            if (mainCamera == null) return;

            var voiceUi = Instantiate(voiceUIPrefab, mainCamera.transform);
            voiceUi.GetComponent<Canvas>().worldCamera = Camera.main;

            _micImage = voiceUi.transform.GetChild(0).gameObject.GetComponent<Image>();
            _micImage.enabled = false;
        }

        _micImage.enabled = photonVoiceView.IsRecording;
        speakerImage.enabled = photonVoiceView.IsSpeaking;
    }
}