using NuiN.NExtensions;
using NuiN.NExtensions.Editor;
using UnityEngine;

public class FloorClippingResolver : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask floorMask;

    [SerializeField] float checkDistUp = 10f;
    [SerializeField] float checkDistDown = Mathf.Infinity;
    [SerializeField] float additionalResolveHeight = 1f;

    bool _isClippingInMesh;
    bool _isClippingUnderMap;

    public void SetRigidbodyTarget(Rigidbody rb)
    {
        this.rb = rb;
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        
        bool hitBelow = RaycastHitDown();
        bool hitAbove = RaycastHitUp(out RaycastHit hit, out bool hitBackface);

        _isClippingInMesh = hitAbove && hitBackface;
        _isClippingUnderMap = hitAbove && !hitBelow && hitBackface;

        if (_isClippingInMesh || _isClippingUnderMap)
        {
            rb.position = rb.position.Add(y: hit.distance + additionalResolveHeight);
        }
    }

    bool RaycastHitUp(out RaycastHit hit, out bool hitBackFace)
    {
        bool queriesHitBackfaces = Physics.queriesHitBackfaces;
        Physics.queriesHitBackfaces = true;
        
        bool hitSomething = Physics.Raycast(rb.position, Vector3.up, out hit, checkDistUp, floorMask);
        hitBackFace = Vector3.Dot(Vector3.up, hit.normal) > 0f;

        Physics.queriesHitBackfaces = queriesHitBackfaces;
        return hitSomething;
    }

    bool RaycastHitDown()
    {
        return Physics.Raycast(rb.position, Vector3.down, out RaycastHit _, checkDistDown, floorMask);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || rb == null) return;
        
        Gizmos.color = Color.black;
        Gizmos.DrawRay(rb.position, Vector3.up * checkDistUp);
        
        GizmoUtils.DrawString($"Inside Mesh: {_isClippingInMesh}", 150, rb.position + Vector3.up * 2f, Color.black);
        GizmoUtils.DrawString($"Under map: {_isClippingUnderMap}", 150, rb.position + Vector3.up * 1.5f, Color.black);
    }
}
