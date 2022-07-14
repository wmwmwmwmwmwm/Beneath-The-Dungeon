using UnityEngine;

public class BillboardMapCamera : MonoBehaviour
{
    Transform MapCameraTransform;

    void Start()
    {
        MapCameraTransform = Player.Instance.MainCamera.transform;
    }

    void Update()
    {
        transform.rotation = MapCameraTransform.rotation;
    }
}
