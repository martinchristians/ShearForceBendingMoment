using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject backButton;

    void Start()
    {
        backButton.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.instance.LeaveRoomAndLoadPreviousRoom);
    }
}