using UnityEngine;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    public GameObject xrOrigin;
    public GameObject desktopCharacter;

    [HideInInspector] public bool isVrPlayer;

    public static GameManager instance;

    private void Awake()
    {
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
            return;
        }

        xrOrigin.SetActive(true);
        desktopCharacter.SetActive(false);

        isVrPlayer = true;
    }
}