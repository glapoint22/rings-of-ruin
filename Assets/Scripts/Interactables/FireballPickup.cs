public class FireballPickup : InteractableBase
{
    private readonly PlayerState fireballUpdate = new()
    {
        hasFireball = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(fireballUpdate);
        GameEvents.RaiseAddSpell(PickupType.Fireball);
    }
}