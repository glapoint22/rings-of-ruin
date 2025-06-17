public class BuffUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        GameEvents.OnShieldPickup += () => AddIcon(PickupType.Shield);
        GameEvents.OnTimeDilationPickup += () => AddIcon(PickupType.TimeDilation);
        GameEvents.OnCloakPickup += () => AddIcon(PickupType.Cloak);
        GameEvents.OnKeyPickup += () => AddIcon(PickupType.Key);
    }
}