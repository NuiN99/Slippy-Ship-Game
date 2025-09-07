using NuiN.NExtensions;
using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] Rigidbody parentRB;
    [SerializeField] float maxForce = 20f;
    [SerializeField] float engineWeight = 5f;
    [SerializeField] float maxTurnAngle = 45f;
    [SerializeField] float adjustThrottleSpeed = 1f;
    [SerializeField] float adjustSteeringSpeed = 1f;
    [SerializeField] float passiveSteeringReturnSpeed = 3f;

    [ShowInInspector] public float CurrentThrottle { get; private set; }
    [ShowInInspector] public float CurrentSteerDirection { get; private set; }

    void Update()
    {
        CurrentSteerDirection = Mathf.MoveTowards(CurrentSteerDirection, 0, passiveSteeringReturnSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        parentRB.AddForceAtPosition(Vector3.down * engineWeight, transform.position, ForceMode.Force);
        
        if (!WaterBuoyancyController.Instance.IsSubmerged(transform.position, out float depth)) return;
        ApplyEngineForce();
    }

    void ApplyEngineForce()
    {
        float force = CurrentThrottle * maxForce;
        Vector3 dir = Quaternion.AngleAxis(maxTurnAngle * -CurrentSteerDirection, transform.up) * transform.forward;
        parentRB.AddForceAtPosition(dir * force, transform.position, ForceMode.Force);
    }

    public void AdjustThrottle(float direction, float deltaTime)
    {
        CurrentThrottle += direction * adjustThrottleSpeed * deltaTime;
        CurrentThrottle = Mathf.Clamp(CurrentThrottle, -1, 1);
    }
    
    public void AdjustSteering(float direction, float deltaTime)
    {
        CurrentSteerDirection += direction * (adjustSteeringSpeed + passiveSteeringReturnSpeed) * deltaTime;
        CurrentSteerDirection = Mathf.Clamp(CurrentSteerDirection, -1, 1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, -transform.forward * 5f);
    }
}