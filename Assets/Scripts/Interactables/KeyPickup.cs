public class KeyPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseKeyPickup();
    }
}