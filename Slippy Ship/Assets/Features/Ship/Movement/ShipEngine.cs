using System;
using NuiN.NExtensions;
using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [Serializable]
    public struct Stats
    {
        public float engineWeight;
        public float maxForce;
        public float maxTurnAngle;
        public float steeringSpeedMult;
        public float adjustThrottleSpeed;
        public float passiveThrottleReturnSpeed;
        public float adjustSteeringSpeed;
        public float passiveSteeringReturnSpeed;
    } 
    
    [SerializeField] Rigidbody parentRB;
    public Stats stats;

    [ShowInInspector] public float CurrentThrottle { get; private set; }
    [ShowInInspector] public float CurrentSteerDirection { get; private set; }

    void Update()
    {
        CurrentSteerDirection = Mathf.MoveTowards(CurrentSteerDirection, 0, stats.passiveSteeringReturnSpeed * Time.deltaTime);
        CurrentThrottle = Mathf.MoveTowards(CurrentThrottle, 0, stats.passiveThrottleReturnSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        parentRB.AddForceAtPosition(Vector3.down * stats.engineWeight, transform.position, ForceMode.Force);
        
        if (!WaterBuoyancyController.Instance.IsSubmerged(transform.position, out float depth)) return;
        ApplyEngineForce();
        ApplySteeringTorque();
    }

    void ApplyEngineForce()
    {
        float force = CurrentThrottle * stats.maxForce;
        Vector3 dir = Quaternion.AngleAxis(stats.maxTurnAngle * -CurrentSteerDirection, transform.up) * transform.forward;
        parentRB.AddForceAtPosition(dir * force, transform.position, ForceMode.Force);
    }
    
    void ApplySteeringTorque()
    {
        if (Mathf.Approximately(CurrentSteerDirection, 0f)) return;

        float torqueForce = stats.maxForce * stats.steeringSpeedMult * CurrentSteerDirection * (1 - Mathf.Abs(CurrentThrottle * 0.25f));
        const float leverArm = 2f;

        Vector3 leftPoint = transform.position - transform.right * leverArm;
        Vector3 rightPoint = transform.position + transform.right * leverArm;

        parentRB.AddForceAtPosition(transform.forward * torqueForce, leftPoint, ForceMode.Force);
        parentRB.AddForceAtPosition(-transform.forward * torqueForce, rightPoint, ForceMode.Force);
    }

    public void AdjustThrottle(float direction, float deltaTime)
    {
        CurrentThrottle += direction * (stats.adjustThrottleSpeed + stats.passiveThrottleReturnSpeed) * deltaTime;
        CurrentThrottle = Mathf.Clamp(CurrentThrottle, -1, 1);
    }
    
    public void AdjustSteering(float direction, float deltaTime)
    {
        CurrentSteerDirection += direction * (stats.adjustSteeringSpeed + stats.passiveSteeringReturnSpeed) * deltaTime;
        CurrentSteerDirection = Mathf.Clamp(CurrentSteerDirection, -1, 1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, -transform.forward * 5f);
    }
}