using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Editor Fields
    public Transform body;
    public Transform cameraCenter;
    public new Transform camera;

    public Vector3 cameraCenterOffset = Vector3.zero;
    public float cameraGrabSensitivity = 200f;
    public float cameraZoomSensitivity = 500f;
    public float maximumVeritcalRotation = 45f;

    public float currentZoom = 5f;
    public float zoomClosest = 1f;
    public float zoomFarthest = 10f;

    public bool controlsEnabled = true;
    #endregion

    #region Public Methods
    public void LookAt(Vector3 worldPos)
    {
        Vector3 toPos = worldPos - camera.position;
        cameraCenter.forward = toPos;
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (controlsEnabled)
        {
            // Use the mouse scroll wheel to change the zoom level
            float deltaZoom = Input.GetAxis("Mouse ScrollWheel") * cameraZoomSensitivity * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom - deltaZoom, zoomClosest, zoomFarthest);

            // Get the mouse movement delta
            float verticalDelta = Input.GetAxis("Mouse Y") * cameraGrabSensitivity * Time.deltaTime * -1f;
            float horizontalDelta = Input.GetAxis("Mouse X") * cameraGrabSensitivity * Time.deltaTime;

            // Clamp the vertical rotation
            float verticalRot = cameraCenter.eulerAngles.x + verticalDelta;
            if (verticalRot > 180) verticalRot -= 360f;
            verticalRot = Mathf.Clamp(verticalRot, -maximumVeritcalRotation, maximumVeritcalRotation);

            // Set the horizontal rotation
            float horizontalRot = cameraCenter.eulerAngles.y + horizontalDelta;

            // Set the euler angles directly
            cameraCenter.eulerAngles = new Vector3(verticalRot, horizontalRot, 0f);
        }

        // Set position of center object and camera
        SetPosition();
    }
    private void OnValidate()
    {
        if (cameraCenter && body && camera)
        {
            SetPosition();
        }
    }
    #endregion

    #region Private Methods
    private void SetPosition()
    {
        // Make sure camera rig is at the body position
        cameraCenter.position = body.position + cameraCenterOffset;

        // Set the local z position of the camera to perform the zoom
        camera.localPosition = new Vector3(camera.localPosition.x, camera.localPosition.y, -currentZoom);
    }
    #endregion
}
