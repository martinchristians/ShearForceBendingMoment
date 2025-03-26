using UnityEngine;
using Photon.Pun;

public class SessionDataManager : MonoBehaviour
{
    public Session[] sessions;
    public Session activeSession;
    public Section[] sections;
    public Section activeSection;
    public InfoHintData[] infoHintDatas;
    public InfoHintData activeHintData;

    public static SessionDataManager instance;

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
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY,
                    out object mapType))
            {
                if ((string)mapType == "experiment") return;

                var numSectionsInSession1 = sessions[0].sections.Count;
                var numSectionsInSession2 = sessions[1].sections.Count;
                switch ((string)mapType)
                {
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE1:
                        activeSession = sessions[0];
                        activeSection = activeSession.sections[0];
                        activeHintData = infoHintDatas[0];
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE2:
                        activeSession = sessions[1];
                        activeSection = activeSession.sections[0];
                        activeHintData = infoHintDatas[numSectionsInSession1];
                        break;
                    case MultiplayerVRConstants.MAP_TYPE_VALUE_EXERCISE3:
                        activeSession = sessions[2];
                        activeSection = activeSession.sections[0];
                        activeHintData = infoHintDatas[numSectionsInSession1 + numSectionsInSession2];
                        break;
                }
            }
        }
    }
}