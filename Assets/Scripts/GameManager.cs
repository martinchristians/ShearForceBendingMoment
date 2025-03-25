using UnityEngine;
using UnityEngine.XR.Management;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject xrOrigin;
    public Camera xrOriginCamera;
    public GameObject desktopCharacter;
    public Camera desktopCharacterCamera;

    public Canvas[] _canvas;

    [Header("Exercise")] [SerializeField] private Session[] sessions;
    public Session activeSession;

    public static GameManager instance;

    private void Awake()
    {
        _canvas = FindObjectsOfType<Canvas>(true);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null) return;

        var xrManager = xrSettings.Manager;
        if (xrManager == null) return;

        var xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            Debug.Log("Play in Desktop mode");
            xrOrigin.SetActive(false);
            desktopCharacter.SetActive(true);

            AssignCamera(desktopCharacterCamera);
        }
        else
        {
            xrOrigin.SetActive(true);
            desktopCharacter.SetActive(false);

            AssignCamera(xrOriginCamera);
        }

        //Assign the correct session
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY,
                    out object mapType))
            {
                if ((string)mapType == "experiment") return;

                switch ((string)mapType)
                {
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE1:
                        activeSession = sessions[0];
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE2:
                        activeSession = sessions[1];
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE3:
                        activeSession = sessions[2];
                        break;
                }
            }
        }
    }

    private void AssignCamera(Camera cam)
    {
        foreach (var c in _canvas)
            c.worldCamera = cam;
    }
}