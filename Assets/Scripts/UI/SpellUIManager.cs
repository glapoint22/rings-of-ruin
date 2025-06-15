public class SpellUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        InteractableManager.OnSpellActivated += AddIcon;
        // InteractableManager.OnSpellDeactivated += RemoveIcon;
    }

    protected override void UnsubscribeFromEvents()
    {
        InteractableManager.OnSpellActivated -= AddIcon;
        // InteractableManager.OnSpellDeactivated -= RemoveIcon;
    }
}