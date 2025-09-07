using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform container;
    [SerializeField] Rigidbody parentRB;

    [Header("Mouse Settings")]
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float maxPitch = 60f;
    [SerializeField] float maxYaw = 90f;

    [Header("Camera Inertia Settings")]
    [SerializeField] float pitchVelocityMultiplier = 0.5f;
    [SerializeField] float rollVelocityMultiplier = 0.5f;
    [SerializeField] float velocityDamping = 5f;
    [SerializeField] float waveBobAmplitude = 1f;
    [SerializeField] float waveBobFrequency = 1f;

    float _pitch;
    float _yaw;

    Vector3 _smoothedOffset;

    void LateUpdate()
    {
        HandleMouseLook();
        HandleVelocityEffect();
    }

    void HandleMouseLook()
    {
        Vector2 mouseInput = PlayerInputManager.Controls.Actions.Camera.ReadValue<Vector2>() * sensitivity;

        _pitch -= mouseInput.y;
        _pitch = Mathf.Clamp(_pitch, -maxPitch, maxPitch);

        _yaw += mouseInput.x;
        _yaw = Mathf.Clamp(_yaw, -maxYaw, maxYaw);

        container.localRotation = Quaternion.Euler(0f, _yaw, 0f);
    }

    void HandleVelocityEffect()
    {
        Vector3 localVel = container.InverseTransformDirection(parentRB.linearVelocity);

        float targetPitchOffset = -localVel.z * pitchVelocityMultiplier + waveBobAmplitude * Mathf.Sin(Time.time * waveBobFrequency);
        float targetRollOffset = localVel.x * rollVelocityMultiplier;

        _smoothedOffset = Vector3.Lerp(_smoothedOffset, new Vector3(targetPitchOffset, 0f, targetRollOffset), Time.deltaTime * velocityDamping);

        transform.localRotation = Quaternion.Euler(_pitch + _smoothedOffset.x, 0f, _smoothedOffset.z);
    }
}
