public class SpellUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        GameEvents.OnFireballPickup += () => AddIcon(PickupType.Fireball);
        GameEvents.OnStormboltPickup += () => AddIcon(PickupType.Stormbolt);
        GameEvents.OnBloodrootPickup += () => AddIcon(PickupType.Bloodroot);
    }
}