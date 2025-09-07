using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEngine;

public class BuoyancyPoint : MonoBehaviour
{
    public static HashSet<BuoyancyPoint> BuoyancyPoints { get; private set; } = new();

    [SerializeField, InjectComponent] Rigidbody parentRB;
    [Tooltip("Estimated displacement volume for this point (m^3). Used for buoyancy calculation.")]
    [field: SerializeField] public float EstimatedDisplacementVolume { get; private set; } = 0.5f;

    public Vector3 ParentVelocity => parentRB.linearVelocity;
    public Rigidbody ParentRB => parentRB;

    void OnEnable() => BuoyancyPoints.Add(this);
    void OnDisable() => BuoyancyPoints.Remove(this);

    public void AddForce(Vector3 force, ForceMode mode)
    {
        parentRB.AddForceAtPosition(force, transform.position, mode);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan.WithAlpha(0.9f);
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}