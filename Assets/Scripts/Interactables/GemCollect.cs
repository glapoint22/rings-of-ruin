public class GemCollect : InteractableBase
{
    private readonly PlayerState gemUpdate = new()
    {
        gems = 1
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseCollect(gemUpdate);
        GameEvents.RaiseCollectionUpdate(gemUpdate);
    }
}