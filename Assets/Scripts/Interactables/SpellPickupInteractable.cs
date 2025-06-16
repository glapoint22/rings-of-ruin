using System;

public abstract class SpellPickupInteractable : PickupInteractable
{
    public static event Action<PickupType> OnSpellActivated;
    public static event Action<PickupType> OnSpellDeactivated;

    protected override void OnInteract()
    {
        ActivateSpell();
        Remove();
    }


    protected void ActivateSpell()
    {
        OnSpellActivated?.Invoke(PickupType);
    }

    protected void DeactivateSpell()
    {
        OnSpellDeactivated?.Invoke(PickupType);
    }
}