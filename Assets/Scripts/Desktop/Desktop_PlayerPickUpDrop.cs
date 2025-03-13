using UnityEngine;

public class Desktop_PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private LayerMask layerMask;

    const float MaxRayCastDistance = 30f;

    private NetworkedGrabbing _networkedGrabbing;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_networkedGrabbing == null)
            {
                if (!Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit raycastHit,
                        MaxRayCastDistance, layerMask)) return;

                if (raycastHit.transform.TryGetComponent(out _networkedGrabbing))
                    _networkedGrabbing.OnSelectEntered(mainCamera);
            }
            else
            {
                _networkedGrabbing.OnSelectExited();
                _networkedGrabbing = null;
            }
        }
    }
}