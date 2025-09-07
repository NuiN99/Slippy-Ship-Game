public interface IInteractable
{
    public bool IsInteractable { get; }
    
    public void Interact();
    public void StartHover();
    public void StopHover();
}