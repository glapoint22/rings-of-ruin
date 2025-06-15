public class GemCollect : InteractableBase
{

    public override void Interact()
    {
        Collect(CollectibleType.Gem);
        base.Interact();
    }
}