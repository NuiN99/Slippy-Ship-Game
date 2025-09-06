using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public LayerMask GroundMask => groundMask;

    [SerializeField] Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
    [SerializeField] LayerMask groundMask;

    public event Action OnLand = delegate { };

    void Update()
    {
        bool wasInAir = IsGrounded == false;
        
        IsGrounded = Physics.OverlapBox(transform.TransformPoint(bounds.center), bounds.extents/2, transform.rotation, groundMask).Length > 0;
        
        if (wasInAir && IsGrounded)
        {
            OnLand.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.cyan : Color.blue;
        Gizmos.DrawWireCube(transform.TransformPoint(bounds.center), bounds.extents);
    }
}
