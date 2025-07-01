public class GemCollect : InteractableBase
{
    private readonly PlayerState gemCollected = new()
    {
        gems = 1
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseAddCollectible(gemCollected);
        GameEvents.RaisePlayerStateUpdate(gemCollected);
    }
}