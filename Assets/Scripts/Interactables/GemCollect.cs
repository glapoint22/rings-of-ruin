public class GemCollect : InteractableBase
{
    private readonly Gem gem = new();

    protected override void OnInteract()
    {
        GameEvents.RaiseCollect(gem);
    }
}