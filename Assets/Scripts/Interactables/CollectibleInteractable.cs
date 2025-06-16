public abstract class CollectibleInteractable : InteractableBase
{
    protected abstract CollectibleType CollectibleType { get; }
    protected override void OnInteract()
    {
        Remove();
    }
}