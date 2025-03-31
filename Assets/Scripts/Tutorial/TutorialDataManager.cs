using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Navigation
{
    public GameObject section;
    public bool isDone;
}


public class TutorialDataManager : MonoBehaviour
{
    public int currentActive;
    public GameObject menuNavigation;
    public PlayStopAudio playStopAudio;
    public Navigation[] navigations;

    [Header("UI")] public GameObject panel;
    public GameObject playStopAudioButton;
    private Vector3 _panelInitPos;
    private Quaternion _panelInitRot;
    private Vector3 _panelInitScale;
    public Image image;
    public Image imageLong;
    public TextMeshProUGUI textMeshProUGUI;

    public static TutorialDataManager instance;

    private void Awake()
    {
        _panelInitPos = panel.transform.position;
        _panelInitRot = panel.transform.rotation;
        _panelInitScale = panel.transform.localScale;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void CheckFirstTime()
    {
        if (!menuNavigation)
        {
            Debug.LogWarning("ButtonNavigation is not set yet");
            return;
        }

        menuNavigation.SetActive(navigations[currentActive].isDone);
    }

    /*public void SetUIInitialValue()
    {
        panel.transform.position = _panelInitPos;
        panel.transform.rotation = _panelInitRot;
        panel.transform.localScale = _panelInitScale;

        Color color = image.color;
        color.a = 0;
        image.color = color;

        Color color2 = imageLong.color;
        color2.a = 0;
        imageLong.color = color2;

        Color color3 = textMeshProUGUI.color;
        color3.a = 0;
        textMeshProUGUI.color = color3;
    }*/
}