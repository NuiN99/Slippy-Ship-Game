using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float maxPitch = 60f;
    [SerializeField] float maxYaw = 90f;

    float _pitch;
    float _yaw;

    void LateUpdate()
    {
        Vector2 mouseInput = PlayerInputManager.Controls.Actions.Camera.ReadValue<Vector2>() * (sensitivity);

        _pitch -= mouseInput.y;
        _pitch = Mathf.Clamp(_pitch, -maxPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

        _yaw += mouseInput.x;
        _yaw = Mathf.Clamp(_yaw, -maxYaw, maxYaw);
        container.localRotation = Quaternion.Euler(0f, _yaw, 0f);
    }
}
