public class GemCollect : InteractableBase
{

    public override void Interact()
    {
        InteractEventManager.CollectGem();
        base.Interact();
    }
}