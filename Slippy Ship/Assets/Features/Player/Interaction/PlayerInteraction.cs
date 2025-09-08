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

    void OnEnable()
    {
        Instance = this;
        PlayerInputManager.Controls.Actions.Interact.performed += OnInteractPressed_Callback;
    }

    void OnDisable()
    {
        PlayerInputManager.Controls.Actions.Interact.performed -= OnInteractPressed_Callback;
    }

    void LateUpdate()
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
            
            float dist = (hit.point - cameraTransform.position).sqrMagnitude;
            if (dist >= closestDistance) continue;
            
            closestInteractable = interactable;
            closestDistance = dist;
        }
        
        return closestInteractable;
    }
}