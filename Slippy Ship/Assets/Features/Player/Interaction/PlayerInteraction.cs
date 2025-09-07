using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance { get; private set; }

    public bool IsHovering => _hoveredInteractable != null;
    
    [SerializeField] Transform cameraTransform;
    [SerializeField] float interactRadius = 1f;
    [SerializeField] float interactRange = 250f;

    IInteractable _hoveredInteractable;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        PlayerInputManager.Controls.Actions.Interact.performed += OnInteractPressed_Callback;
    }

    void OnDisable()
    {
        PlayerInputManager.Controls.Actions.Interact.performed -= OnInteractPressed_Callback;
    }

    void Update()
    {
        IInteractable detectedInteractable = DetectInteractable();
        if (detectedInteractable == _hoveredInteractable) return;
        
        _hoveredInteractable?.StopHover();
        _hoveredInteractable = detectedInteractable;
        _hoveredInteractable?.StartHover();
    }

    void OnInteractPressed_Callback(InputAction.CallbackContext ctx)
    {
        _hoveredInteractable?.Interact();
    }

    IInteractable DetectInteractable()
    {
        RaycastHit[] hits = Physics.SphereCastAll(cameraTransform.position, interactRadius, cameraTransform.forward, interactRange);
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.TryGetComponent(out IInteractable interactable) || !interactable.IsInteractable) continue;
            if (Physics.Raycast(cameraTransform.position, VectorUtils.Direction(cameraTransform.position, hit.collider.transform.position), out RaycastHit losCheckHit))
            {
                // hit something in line of sight (los)
                if(losCheckHit.collider != hit.collider) continue;
            }
            
            float dist = (hit.point - cameraTransform.position).sqrMagnitude;
            if (dist >= closestDistance) continue;
            
            closestInteractable = interactable;
            closestDistance = dist;
        }
        
        return closestInteractable;
    }
    
    void OnDrawGizmos()
    {
        Vector3 start = cameraTransform.position;
        Vector3 end = cameraTransform.position + cameraTransform.forward * interactRange;
        Gizmos.DrawWireSphere(start, interactRadius);
        Gizmos.DrawWireSphere(end, interactRadius);
        Gizmos.DrawLine(start, end);
    }
}