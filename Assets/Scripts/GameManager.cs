using UnityEngine;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    public GameObject xrOrigin;
    public Camera xrOriginCamera;
    public GameObject desktopCharacter;
    public Camera desktopCharacterCamera;

    public Canvas[] _canvas;

    [Header("Exercise")] public Session session;

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
    }

    private void AssignCamera(Camera cam)
    {
        foreach (var c in _canvas)
            c.worldCamera = cam;
    }
}