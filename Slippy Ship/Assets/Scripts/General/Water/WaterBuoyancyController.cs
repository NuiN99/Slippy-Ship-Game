using System.Collections;
using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterBuoyancyController : MonoBehaviour
{
    public static WaterBuoyancyController Instance { get; private set; }
    
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
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

    public bool IsSubmerged(Vector3 position, out float depth)
    {
        depth = 0f;
        
        _sp.startPositionWS = _sr.candidateLocationWS;
        _sp.targetPositionWS = position;

        if (!targetSurface.ProjectPointOnWaterSurface(_sp, out _sr))
            return false;

        float waterY = _sr.projectedPositionWS.y;

        depth = waterY - position.y;
        return depth >= EPSILON;
    }

    void ApplyBuoyancy(BuoyancyPoint bp)
    {
        if (!IsSubmerged(bp.transform.position, out float depth))
        {
            return;
        }
        
        float submergedRatio = Mathf.Clamp01(depth / maxSubmergeDepth);

        float displacedMass = submergedRatio * bp.EstimatedDisplacementVolume; 
        Vector3 buoyantForce = _sr.normalWS * (displacedMass * waterDensity);
        bp.AddForce(buoyantForce, ForceMode.Force);

        if (bp.AffectedByCurrent)
        {
            Vector3 swellCurrentVel = new Vector3(0, 0f, targetSurface.largeOrientationValue).normalized * targetSurface.largeCurrentSpeedValue;
            Vector3 currentForce = swellCurrentVel * (waterDensity * submergedRatio * 0.01f);
            bp.AddForce(currentForce, ForceMode.Force);
        }

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
