using Photon.Pun;
using UnityEngine;

public class AvatarSelectionManager : MonoBehaviour
{
    public AvatarInputConverter avatarInputConverter;

    [HideInInspector] public int avatarHeadSelectionNumber;
    [HideInInspector] public int avatarBodySelectionNumber;

    public GameObject[] avatarHeadPlayerModels;
    public GameObject[] avatarBodyPlayerModels;

    public static AvatarSelectionManager instance;

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
        //Display selected avatar head model
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_HEAD_SELECTION_NUMBER,
                out var storedAvatarHeadSelectionNumber))
        {
            Debug.Log("Stored avatar head selection number: " + (int)storedAvatarHeadSelectionNumber);
            avatarHeadSelectionNumber = (int)storedAvatarHeadSelectionNumber;
        }
        else
            avatarHeadSelectionNumber = 0;

        LoadAvatarHeadModelAt(avatarHeadSelectionNumber);

        //Display selected avatar body model
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_BODY_SELECTION_NUMBER,
                out var storedAvatarBodySelectionNumber))
        {
            Debug.Log("Stored avatar body selection number: " + (int)storedAvatarBodySelectionNumber);
            avatarBodySelectionNumber = (int)storedAvatarBodySelectionNumber;
        }
        else
            avatarHeadSelectionNumber = 0;

        LoadAvatarBodyModelAt(avatarBodySelectionNumber);
    }

    public void NextAvatarHead()
    {
        avatarHeadSelectionNumber += 1;
        if (avatarHeadSelectionNumber >= avatarHeadPlayerModels.Length)
            avatarHeadSelectionNumber = 0;

        LoadAvatarHeadModelAt(avatarHeadSelectionNumber);
    }

    public void PreviousAvatarHead()
    {
        avatarHeadSelectionNumber -= 1;
        if (avatarHeadSelectionNumber < 0)
            avatarHeadSelectionNumber = avatarHeadPlayerModels.Length - 1;

        LoadAvatarHeadModelAt(avatarHeadSelectionNumber);
    }

    public void NextAvatarBody()
    {
        avatarBodySelectionNumber += 1;
        if (avatarBodySelectionNumber >= avatarBodyPlayerModels.Length)
            avatarBodySelectionNumber = 0;

        LoadAvatarBodyModelAt(avatarBodySelectionNumber);
    }

    public void PreviousAvatarBody()
    {
        avatarBodySelectionNumber -= 1;
        if (avatarBodySelectionNumber < 0)
            avatarBodySelectionNumber = avatarBodyPlayerModels.Length - 1;

        LoadAvatarBodyModelAt(avatarBodySelectionNumber);
    }

    private void LoadAvatarHeadModelAt(int avatarHeadIndex)
    {
        foreach (GameObject headModelPlayer in avatarHeadPlayerModels)
            headModelPlayer.SetActive(false);
        avatarHeadPlayerModels[avatarHeadIndex].SetActive(true);
        avatarHeadPlayerModels[avatarHeadIndex].layer = 6;

        avatarInputConverter.avatarHead = avatarHeadPlayerModels[avatarHeadIndex].GetComponent<Transform>();

        ExitGames.Client.Photon.Hashtable playerSelectionProperty = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.AVATAR_HEAD_SELECTION_NUMBER, avatarHeadSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }

    private void LoadAvatarBodyModelAt(int avatarBodyIndex)
    {
        foreach (GameObject bodyModelPlayer in avatarBodyPlayerModels)
            bodyModelPlayer.SetActive(false);
        avatarBodyPlayerModels[avatarBodyIndex].SetActive(true);

        avatarInputConverter.avatarBody = avatarBodyPlayerModels[avatarBodyIndex].GetComponent<Transform>();

        ExitGames.Client.Photon.Hashtable playerSelectionProperty = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.AVATAR_BODY_SELECTION_NUMBER, avatarBodySelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }
}