using System;
using System.Collections.Generic;
using System.Linq;
using NuiN.NExtensions;
using UnityEngine;

public class GroundRaycaster : MonoBehaviour
{
    public event Action OnLand = delegate { };
    public LayerMask GroundMask => groundMask;
    
    [ShowInInspector] public bool IsGrounded { get; private set; }
    [ShowInInspector] public Vector3 GroundNormal { get; private set; }
    public bool AlignmentRayHitGround { get; private set; }
    public bool FrontAlignmentRayHitGround { get; private set; }
    
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask alignMask;

    [SerializeField] Vector3 groundCheckBoxSize;
    [SerializeField] Vector3 groundCheckBoxOffset;
        
    [SerializeField] Transform alignRaysBottomParent;
    [SerializeField] Transform alignRaysFrontParent;
    
    [SerializeField] float smoothNormalStep = 10f;
    
    List<Collider> _collidersToIgnore = new();
    bool _isDisabled;
    Coroutine _disableForDurationRoutine;
    Vector3 _smoothedNormal = Vector3.up;

    void Start()
    {
        List<Collider> parentCols = GetComponentsInParent<Collider>().ToList();
        List<Collider> childCols = GetComponentsInChildren<Collider>().ToList();
        
        _collidersToIgnore.AddRange(parentCols);
        _collidersToIgnore.AddRange(childCols);
    }

    void Update()
    {
        GroundNormal = Vector3.up;
        AlignmentRayHitGround = false;
        FrontAlignmentRayHitGround = false;
        int count = 0;
        
        if (_isDisabled)
        {
            return;
        }

        bool hitTriggers = Physics.queriesHitTriggers;
        Physics.queriesHitTriggers = false;

        Vector3 boxSize = Vector3.Scale(groundCheckBoxSize, transform.lossyScale);
        
        bool wasGrounded = IsGrounded;
        if (groundCheckBoxSize.sqrMagnitude != 0
            && Physics.OverlapBox(transform.TransformPoint(groundCheckBoxOffset), boxSize / 2f, transform.rotation, groundMask).Any(col => !_collidersToIgnore.Contains(col)))
        {
            IsGrounded = true;
            
            if (wasGrounded == false)
            {
                OnLand.Invoke();
            }
        }
        else
        {
            IsGrounded = false;
        }
        
        foreach (Transform ts in alignRaysBottomParent)
        {
            if (!AlignmentHitDown(ts, out RaycastHit hit)) continue;
            if (!alignMask.ContainsLayer(hit.collider)) continue;
            
            GroundNormal += hit.normal;
            count++;
            
            AlignmentRayHitGround = true;
        }
        
        foreach (Transform ts in alignRaysFrontParent)
        {
            if (!AlignmentHitDown(ts, out RaycastHit hit)) continue;
            if (!alignMask.ContainsLayer(hit.collider)) continue;
            
            GroundNormal += hit.normal;
            count++;
            
            FrontAlignmentRayHitGround = true;
        }
        
        Vector3 targetNormal = (count == 0 ? Vector3.up : (GroundNormal / count).normalized);
        _smoothedNormal = Vector3.Lerp(_smoothedNormal, targetNormal, Time.deltaTime * smoothNormalStep);
        GroundNormal = _smoothedNormal;
        
        Physics.queriesHitTriggers = hitTriggers;
    }
    
    public void Enable()
    {
        this.StopCoroutineSafe(_disableForDurationRoutine);
        _isDisabled = false;
    }

    public void Disable()
    {
        IsGrounded = false;
        _isDisabled = true;
    }

    public void DisableForDuration(float duration)
    {
        Disable();

        this.StopCoroutineSafe(_disableForDurationRoutine);
        _disableForDurationRoutine = this.DoAfter(duration, Enable);
    }

    bool AlignmentHitDown(Transform ts, out RaycastHit hit)
    {
        bool didHit = Physics.Raycast(ts.position, ts.up, out hit, ts.lossyScale.y, groundMask);
        return didHit && !_collidersToIgnore.Contains(hit.collider);
    }

    void OnDrawGizmos()
    {
        foreach (Transform ts in alignRaysBottomParent)
        {
            DrawAlignRay(ts, false);
        }

        foreach (Transform ts in alignRaysFrontParent)
        {
            DrawAlignRay(ts, true);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GroundNormal);
        
        Gizmos.color = (IsGrounded ? Color.blue : Color.green).WithAlpha(0.2f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(groundCheckBoxOffset, groundCheckBoxSize);
        
        return;
        void DrawAlignRay(Transform ts, bool front)
        {
            if (AlignmentHitDown(ts, out RaycastHit hit))
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(ts.position, hit.point);
            }
            else
            {
                Gizmos.color = front ? Color.yellow : Color.blue;
                Gizmos.DrawRay(ts.position, ts.up * ts.lossyScale.y);
            }
        }
    }
}