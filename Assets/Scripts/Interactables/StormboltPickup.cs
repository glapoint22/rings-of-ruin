public class StormboltPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseStormboltPickup();
    }
}