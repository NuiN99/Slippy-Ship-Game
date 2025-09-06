using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] Rigidbody parentRB;
    [SerializeField] float maxForce = 20f;
    [SerializeField] float maxTurnAngle = 45f;
    [SerializeField] float adjustThrottleSpeed = 1f;
    [SerializeField] float adjustSteeringSpeed = 1f;
    
    [SerializeField, Range(-1, 1)] float currentThrottle = 0f;
    [SerializeField, Range(-1, 1)] float currentSteerDirection = 0f;
    
    void FixedUpdate()
    {
        if (!WaterBuoyancyController.Instance.IsSubmerged(transform.position, out float depth)) return;
        ApplyEngineForce();
    }

    void ApplyEngineForce()
    {
        float force = currentThrottle * maxForce;
        Vector3 dir = Quaternion.AngleAxis(maxTurnAngle * -currentSteerDirection, transform.up) * transform.forward;
        parentRB.AddForceAtPosition(dir * force, transform.position, ForceMode.Force);
    }

    public void AdjustThrottle(float direction, float deltaTime)
    {
        currentThrottle += direction * adjustThrottleSpeed * deltaTime;
        currentThrottle = Mathf.Clamp(currentThrottle, -1, 1);
    }
    
    public void AdjustSteering(float direction, float deltaTime)
    {
        currentSteerDirection += direction * adjustSteeringSpeed * deltaTime;
        currentSteerDirection = Mathf.Clamp(currentSteerDirection, -1, 1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, -transform.forward * 5f);
    }
}