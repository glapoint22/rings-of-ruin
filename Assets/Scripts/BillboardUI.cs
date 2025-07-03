using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Make the UI face the camera
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}