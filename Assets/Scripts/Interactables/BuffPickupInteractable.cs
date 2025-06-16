using System;

public abstract class BuffPickupInteractable : PickupInteractable
{
    public static event Action<PickupType> OnBuffActivated;
    public static event Action<PickupType> OnBuffDeactivated;

    protected override void OnInteract()
    {
        ActivateBuff();
        Remove();
    }


    protected void ActivateBuff()
    {
        OnBuffActivated?.Invoke(PickupType);
    }

    protected void DeactivateBuff()
    {
        OnBuffDeactivated?.Invoke(PickupType);
    }
}