public class FireballPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseFireballPickup();
    }
}