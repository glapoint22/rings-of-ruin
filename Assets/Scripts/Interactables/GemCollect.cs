public class GemCollect : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseGemCollect();
    }
}