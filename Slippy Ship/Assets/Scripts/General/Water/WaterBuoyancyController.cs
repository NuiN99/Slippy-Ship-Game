using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterBuoyancyController : MonoBehaviour
{
    const float EPSILON = 0.02f;

    [Header("Water")]
    [SerializeField] WaterSurface targetSurface;

    [Header("Physics")]
    [SerializeField] float waterDensity = 1027f;
    [SerializeField] float maxSubmergeDepth = 2f;

    [Header("Drag")]
    [SerializeField] float linearDrag = 1.5f;
    [SerializeField] float angularDrag = 0.8f;

    [Header("Stability")]
    [SerializeField] float uprightTorqueStrength = 5f;

    WaterSearchParameters _sp;
    WaterSearchResult _sr;

    void Awake()
    {
        _sp.error = 0.01f;
        _sp.maxIterations = 8;
        _sp.includeDeformation = true;
        _sp.excludeSimulation = false;
    }

    void FixedUpdate()
    {
        foreach (BuoyancyPoint bp in BuoyancyPoint.BuoyancyPoints)
        {
            ApplyBuoyancy(bp);
        }
    }

    void ApplyBuoyancy(BuoyancyPoint bp)
    {
        _sp.startPositionWS = _sr.candidateLocationWS;
        _sp.targetPositionWS = bp.transform.position;

        if (!targetSurface.ProjectPointOnWaterSurface(_sp, out _sr))
            return;

        float waterY = _sr.projectedPositionWS.y;
        Vector3 pos = bp.transform.position;

        float depth = waterY - pos.y;
        if (depth < EPSILON)
            return; // not submerged

        float submergedRatio = Mathf.Clamp01(depth / maxSubmergeDepth);

        float displacedMass = submergedRatio * bp.EstimatedDisplacementVolume; 
        Vector3 buoyantForce = Vector3.up * (displacedMass * waterDensity);

        bp.AddForce(buoyantForce, ForceMode.Force);

        Vector3 relVelocity = bp.ParentVelocity;
        Vector3 dragForce = -relVelocity * (linearDrag * submergedRatio);
        bp.AddForce(dragForce, ForceMode.Force);

        bp.ParentRB.angularVelocity *= (1f - angularDrag * submergedRatio * Time.fixedDeltaTime);

        Vector3 up = bp.ParentRB.transform.up;
        Vector3 torqueAxis = Vector3.Cross(up, Vector3.up);
        Vector3 correctiveTorque = torqueAxis * (uprightTorqueStrength * submergedRatio);
        bp.ParentRB.AddTorque(correctiveTorque, ForceMode.Acceleration);
    }
}
