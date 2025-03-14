using Photon.Pun;
using UnityEngine;

public class AvatarSelectionManager : MonoBehaviour
{
    public GameObject[] avatarHeadPlayerModels;
    public GameObject[] avatarBodyPlayerModels;

    private AvatarInputConverter _avatarInputConverter;
    private int _avatarHeadSelectionNumber;
    private int _avatarBodySelectionNumber;

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
        _avatarInputConverter = FindObjectOfType<AvatarInputConverter>();

        //Display selected avatar head model
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_HEAD_SELECTION_NUMBER,
                out var storedAvatarHeadSelectionNumber))
        {
            Debug.Log("Stored avatar head selection number: " + (int)storedAvatarHeadSelectionNumber);
            _avatarHeadSelectionNumber = (int)storedAvatarHeadSelectionNumber;
        }
        else
            _avatarHeadSelectionNumber = 0;

        LoadAvatarHeadModelAt(_avatarHeadSelectionNumber);

        //Display selected avatar body model
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_BODY_SELECTION_NUMBER,
                out var storedAvatarBodySelectionNumber))
        {
            Debug.Log("Stored avatar body selection number: " + (int)storedAvatarBodySelectionNumber);
            _avatarBodySelectionNumber = (int)storedAvatarBodySelectionNumber;
        }
        else
            _avatarHeadSelectionNumber = 0;

        LoadAvatarBodyModelAt(_avatarBodySelectionNumber);
    }

    public void NextAvatarHead()
    {
        _avatarHeadSelectionNumber += 1;
        if (_avatarHeadSelectionNumber >= avatarHeadPlayerModels.Length)
            _avatarHeadSelectionNumber = 0;

        LoadAvatarHeadModelAt(_avatarHeadSelectionNumber);
    }

    public void PreviousAvatarHead()
    {
        _avatarHeadSelectionNumber -= 1;
        if (_avatarHeadSelectionNumber < 0)
            _avatarHeadSelectionNumber = avatarHeadPlayerModels.Length - 1;

        LoadAvatarHeadModelAt(_avatarHeadSelectionNumber);
    }

    public void NextAvatarBody()
    {
        _avatarBodySelectionNumber += 1;
        if (_avatarBodySelectionNumber >= avatarBodyPlayerModels.Length)
            _avatarBodySelectionNumber = 0;

        LoadAvatarBodyModelAt(_avatarBodySelectionNumber);
    }

    public void PreviousAvatarBody()
    {
        _avatarBodySelectionNumber -= 1;
        if (_avatarBodySelectionNumber < 0)
            _avatarBodySelectionNumber = avatarBodyPlayerModels.Length - 1;

        LoadAvatarBodyModelAt(_avatarBodySelectionNumber);
    }

    private void LoadAvatarHeadModelAt(int avatarHeadIndex)
    {
        foreach (GameObject headModelPlayer in avatarHeadPlayerModels)
            headModelPlayer.SetActive(false);
        avatarHeadPlayerModels[avatarHeadIndex].SetActive(true);
        avatarHeadPlayerModels[avatarHeadIndex].layer = 6;

        _avatarInputConverter.avatarHead = avatarHeadPlayerModels[avatarHeadIndex].GetComponent<Transform>();

        ExitGames.Client.Photon.Hashtable playerSelectionProperty = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.AVATAR_HEAD_SELECTION_NUMBER, _avatarHeadSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }

    private void LoadAvatarBodyModelAt(int avatarBodyIndex)
    {
        foreach (GameObject bodyModelPlayer in avatarBodyPlayerModels)
            bodyModelPlayer.SetActive(false);
        avatarBodyPlayerModels[avatarBodyIndex].SetActive(true);

        _avatarInputConverter.avatarBody = avatarBodyPlayerModels[avatarBodyIndex].GetComponent<Transform>();

        ExitGames.Client.Photon.Hashtable playerSelectionProperty = new ExitGames.Client.Photon.Hashtable()
            { { MultiplayerVRConstants.AVATAR_BODY_SELECTION_NUMBER, _avatarBodySelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }
}