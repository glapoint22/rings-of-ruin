public class GemCollect : InteractableBase
{

    public override void Interact()
    {
        InteractEventManager.Collect(CollectibleType.Gem);
        base.Interact();
    }
}