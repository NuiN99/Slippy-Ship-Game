using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseInteractable : MonoBehaviour, IInteractable
{
    public virtual bool IsInteractable => true;
    
    [SerializeField] Material hoverMaterial;
    [SerializeField] MeshRenderer[] renderers;
    
    Dictionary<MeshRenderer, Material> _cachedMaterialReferences = new();

    void OnValidate()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    protected virtual void Awake()
    {
        foreach (var r in renderers)
        {
            _cachedMaterialReferences.Add(r, r.material);
        }
    }
    
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + name, this);
    }

    public virtual void StartHover()
    {
        foreach (var r in renderers)
        {
            r.material = hoverMaterial;
        }
    }

    public virtual void StopHover()
    {
        foreach (var r in renderers)
        {
            if (_cachedMaterialReferences.TryGetValue(r, out Material originalMaterial))
            {
                r.material = originalMaterial;
            }
        }
    }
}
