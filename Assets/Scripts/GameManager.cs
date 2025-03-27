using UnityEngine;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlayerVR;

    public GameObject xrOriginGameObject;
    public GameObject xrOriginPlayer;
    public Camera xrOriginCamera;

    public GameObject desktopCharacterGameObject;
    public GameObject desktopPlayer;
    public Camera desktopPlayerCamera;

    public Canvas[] _canvas;

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
            xrOriginGameObject.SetActive(false);
            desktopCharacterGameObject.SetActive(true);

            AssignCamera(desktopPlayerCamera);
        }
        else
        {
            isPlayerVR = true;
            xrOriginGameObject.SetActive(true);
            desktopCharacterGameObject.SetActive(false);

            AssignCamera(xrOriginCamera);
        }
    }

    private void AssignCamera(Camera cam)
    {
        foreach (var c in _canvas)
            c.worldCamera = cam;
    }
}