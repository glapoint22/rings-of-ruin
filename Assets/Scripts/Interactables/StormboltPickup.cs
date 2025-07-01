public class StormboltPickup : InteractableBase
{
    private readonly PlayerState stormboltUpdate = new()
    {
        hasStormbolt = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(stormboltUpdate);
        GameEvents.RaiseAddSpell(PickupType.Stormbolt);
    }
}