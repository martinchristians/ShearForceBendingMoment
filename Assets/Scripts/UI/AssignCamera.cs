using UnityEngine;

public class AssignCamera : MonoBehaviour
{
    public Camera xrPlayer;
    public Camera desktopPlayer;

    public Canvas[] canvas;

    private void Start()
    {
        if (GameManager.instance.isVrPlayer)
            foreach (var c in canvas)
                c.worldCamera = xrPlayer;
        else
            foreach (var c in canvas)
                c.worldCamera = desktopPlayer;
    }
}