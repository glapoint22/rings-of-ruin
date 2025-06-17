public class BloodrootPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseBloodrootPickup();
    }
}