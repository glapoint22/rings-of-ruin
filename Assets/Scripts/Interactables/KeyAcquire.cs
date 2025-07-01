public class KeyAcquire : InteractableBase
{
    private readonly PlayerState keyAcquired = new()
    {
        hasKey = true
    };
    protected override void OnInteract()
    {
        GameEvents.RaiseKeyPickup();
        GameEvents.RaiseAddBuff(BuffType.Key);
        GameEvents.RaisePlayerStateUpdate(keyAcquired);
    }
}