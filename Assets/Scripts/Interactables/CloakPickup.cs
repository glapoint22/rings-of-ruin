public class CloakPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseCloakPickup();
    }
}