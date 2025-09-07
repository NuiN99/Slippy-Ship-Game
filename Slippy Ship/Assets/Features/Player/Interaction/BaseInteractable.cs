using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseInteractable : MonoBehaviour, IInteractable
{
    public virtual bool IsInteractable => true;
    
    [SerializeField] Material hoverMaterial;
    
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + name, this);
    }

    public virtual void StartHover()
    {
        
    }

    public virtual void StopHover()
    {
        
    }
}
