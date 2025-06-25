public class FireballPickup : InteractableBase
{
    private readonly Fireball fireball = new();

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(fireball, PickupType.Fireball);
    }
}