public class SpellUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        SpellPickupInteractable.OnSpellActivated += AddIcon;
        // InteractableManager.OnSpellDeactivated += RemoveIcon;
    }

    protected override void UnsubscribeFromEvents()
    {
        SpellPickupInteractable.OnSpellActivated -= AddIcon;
        // InteractableManager.OnSpellDeactivated -= RemoveIcon;
    }
}