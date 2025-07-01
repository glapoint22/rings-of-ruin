public class KeyPickup : InteractableBase
{
    private readonly PlayerState keyUpdate = new()
    {
        hasKey = true
    };
    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(keyUpdate);
        GameEvents.RaiseAddBuff(BuffType.Key);
        GameEvents.RaiseKeyPickup();
    }
}