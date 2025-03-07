using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    public GameObject connectOptionsPanelGameobject;
    public GameObject connectWithNamePanelGameobject;

    #region Unity

    void Start()
    {
        connectOptionsPanelGameobject.SetActive(true);
        connectWithNamePanelGameobject.SetActive(false);
    }

    #endregion

    #region Photon Callback

    #endregion
}